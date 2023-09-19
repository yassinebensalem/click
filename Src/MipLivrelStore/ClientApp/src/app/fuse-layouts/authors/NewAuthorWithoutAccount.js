import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	AppBar,
	Box,
	Button,
	Tab,
	Tabs,
	TextField
} from '@material-ui/core';

import { unwrapResult } from '@reduxjs/toolkit';
import React, { useState, useEffect, useRef } from 'react';
import { Link, useHistory } from 'react-router-dom';
import ArrowBack from '@material-ui/icons/ArrowBack';
import a11yProps from '../a11props';
import { useStyles } from '@material-ui/pickers/views/Calendar/SlideTransition';
import user from '../../../images/profile.jpg';
import TabPanel from '../shared-components/TabPanel';
import { KeyboardDatePicker } from '@material-ui/pickers';
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import swal from 'sweetalert';
import moment from 'moment';

import { Visibility, VisibilityOff, Email, Add } from '@material-ui/icons';

import { useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';

import { getCountries } from 'app/store/countrySlice';
import { createAuthorWithoutAcount } from 'app/store/authorWithoutAccountSlice';
import { decode } from 'jsonwebtoken';

const NewAuthorWithoutAccount = () => {
	const [value, setValue] = useState(0);
	const classes = useStyles();
	const [LastName, setLastName] = useState('');
	const [LastNameError, setLastNameError] = useState('');
	const [helperTextLastName, setHelperTextLastName] = useState('');
	const [FirstName, setFirstName] = useState('');
	const [FirstNameError, setFirstNameError] = useState('');
	const [helperTextFirstName, setHelperTextFirstName] = useState('');

	const [selectedDate, setSelectedDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	const [password, setPassword] = useState('');

	const [confirmPassword, setConfirmPassword] = useState('');

	const [email, setEmail] = useState('');
	const [emailError, setEmailError] = useState('');
	const [helperTextEmail, setHelperTextEmail] = useState('');
	const [photoPath, setPhotoPath] = useState('');


	const [countryName, setCountryName] = useState('');
	const [helperTextCountryName, setHelperTextCountryName] = useState(null);
	const [errorCountry, setErrorCountry] = useState(false);


	const [biographie, setBigoraphie] = useState('');
	const [phoneNumber, setPhoneNumber] = useState('');

	const dispatch = useDispatch();
	const { authorsJoined, updated } = useSelector(state => state.authorJoin);
	const { authorsWithoutAccount } = useSelector(state => state.authorWithoutAccount);
	const { countries } = useSelector(state => state.country);
	const [enabled, setEnabled] = useState(false);
	const photoRef = useRef(null);
	const history = useHistory();
	const {token} = useSelector(state => state.auth);
	let regex =
		/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

	const onChangeLastName = e => {
		setLastName(e.target.value);
		setLastNameError(false);
		setHelperTextLastName('');
	};
	useEffect(() => {
		dispatch(getCountries());
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
       }
	}, []);

	
	const getCountryId = name => {
		for (let i = 0; i < countries.length; i++) {
			if (countries[i].name === name) {
				return countries[i].id;
			}
		}
	};

	const findEmail = email => {
		let bool;
		const arr = authorsWithoutAccount.filter(author => author.email === email);

		if (arr.length > 0) {
			bool = true;
		} else {
			bool = false;
		}
		return bool;
	};

	const onChangeFirstName = e => {
		setFirstName(e.target.value);
		setFirstNameError(false);
		setHelperTextFirstName('');
	};

	const onChangeDate = (e, row) => {
		setSelectedDate(row);
	};
	const findFirstName = firstName => {
		let bool;
		const arr = authorsWithoutAccount.filter(author => author.firstName === firstName);
		if (arr.length > 0) {
			bool = true;
		} else {
			bool = false;
		}

		return bool;
	}
	const findLastName = lastName => {
		let bool;
		const arr = authorsWithoutAccount.filter(author => author.lastName === lastName);
		if (arr.length > 0) {
			bool = true;
		} else {
			bool = false;
		}

		return bool;
	}
	const onChangePhotoPath = e => {
		setPhotoPath(e.target.files[0]);
		setEnabled(true);
	};
	const validateCountryError = id => {
		if (id === undefined) {
			setErrorCountry(true);
			setHelperTextCountryName('vous devez choisir un pays');
			setEnabled(false);
		} else {
			setEnabled(true);
		}
	};
	const onChangeCountryName = (e, value) => {
		setCountryName(value.name);
		setErrorCountry(false);
		setHelperTextCountryName('');
	};

	const onChangePhoneNumber = e => {
		setPhoneNumber(e.target.value);
	};
	const validateEmailError = email => {
		if (regex.test(email) === false) {
			setEmailError(true);
			setHelperTextEmail('Vous devez entrer un e-mail valide');
			setEnabled(false);
		} else if (findEmail(email) === true) {
			setEmailError(true);
			setHelperTextEmail('email existant');
			setEnabled(false);
		} else {
			setEnabled(true);
		}
	};
	const validateFirstName = firstName => {
		if (findFirstName(firstName) === true) {
		   setFirstNameError(true);
		   setHelperTextFirstName('prénom existant');
	   }
	   if(firstName === '') {
		   setFirstNameError(true);
		   setHelperTextFirstName('champ vide');
	   } 
	   if(/(^\s+)|(\s+$)/.test(FirstName) === true){
		   setFirstNameError(true);
		   setHelperTextFirstName('le prénom ne doit pas contenir des espaces au début et à la fin');
	   }
   };
   const validateLastName = lastName => {
	if (findLastName(lastName) === true) {
	   setLastNameError(true);
	   setHelperTextLastName('nom existant');
   }
   if(lastName === '') {
	setLastNameError(true);
	setHelperTextLastName('champ vide');
   }
   if(/(^\s+)|(\s+$)/.test(LastName) === true){
	setLastNameError(true);
	setHelperTextLastName('le nom ne doit pas contenir des espaces au début et à la fin');
}}
	const onChangeEmail = e => {
		setEmail(e.target.value);
		setEmailError(false);
		setHelperTextEmail('');
		validateEmailError(e.target.value.trim());
	};

	const onPress = evt => {
		if (
			(evt.which != 8 && evt.which != 0 && evt.which < 48) ||
			evt.which > 57 ||
			evt.target.value.length > 7
		) {
			evt.preventDefault();
		}
	};

	const onChangeBiographie = e => {
		setBigoraphie(e.target.value);
	};

	const reset = () => {
		setFirstName('');
		setLastName('');
		setEmail('');
		setPassword('');
		setConfirmPassword('');
		setCountryName('');
		setBigoraphie('');
		setPhoneNumber('');
		setSelectedDate(moment(new Date()).format('YYYY-MM-DD'));
		photoRef.current.value = null;
		setEnabled(false);
	};

	const onAddAuthor = () => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		const formData = new FormData();
		formData.append('LastName', LastName);
		formData.append('FirstName', FirstName);
		formData.append('Birthdate', selectedDate);
		formData.append('Email', email);
		formData.append('Password', password);
		formData.append('ConfirmPassword', confirmPassword);
		formData.append('Biography', biographie);
		formData.append('CountryId', getCountryId(countryName));
		formData.append('PhotoFile', photoPath);
		formData.append('PhoneNumber', phoneNumber);
		formData.append('UserRole', 3);
		validateCountryError(getCountryId(countryName));
		validateFirstName(FirstName);
		validateLastName(LastName);
		//validateEmailError(email);
		if(FirstName !== "" && LastName !== "" ){
		dispatch(createAuthorWithoutAcount(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				// handle result here
				swal({
					title: 'Auteur sans compte ajouté!',
					icon: 'success'
				});
				reset();
			})
			.catch(rejectedValueOrSerializedError => { });
		}
	};
	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center">
					<div className="pt-10 pb-10">
						<div className="flex flex-col max-w-full min-w-0">
							<Link
								to="/authorWithoutAccount/list"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b className="hidden sm:flex mx-4 font-medium">
									Auteurs via ME
								</b>
							</Link>
						</div>
						<div className="flex items-center max-w-full">
							<img
								src={user}
								alt="user"
								className="w-32 sm:w-48 rounded"
								width={40}
								height={40}
							/>
							<div className="flex flex-col mx-8">
								<h2 className='MuiTypography-root MuiTypography-body1 text-16 sm:text-20 truncate font-semibold muiltr-ehddlr"'>
									{FirstName !== '' || LastName !== ''
										? `${FirstName} ${LastName}`
										: 'Nouvel auteur sans compte'}
								</h2>
								<span className="mt-4 text-xs">
									Détails de l'auteur sans compte
								</span>
							</div>
						</div>
					</div>
					<Button
						onClick={onAddAuthor}
						variant="contained"
						color="primary"
						size="small"
						className="save-btn"
						//disabled={enabled ? false : true}
					>
						Sauvegarder
					</Button>
				</div>
			}
			contentToolbar={
				<div className={classes.root}>
					<AppBar position="static" color="default">
						<Tabs
							value={value}
							indicatorColor="primary"
							textColor="primary"
							variant="scrollable"
							scrollButtons="auto"
							aria-label="scrollable auto tabs example"
						>
							<Tab label="Infos Basiques" {...a11yProps(0)} />
						</Tabs>
					</AppBar>
				</div>
			}
			content={
				<TabPanel value={value} index={0}>
					<Box component="form">
						<TextField
							id="outlined-basic"
							onChange={onChangeFirstName}
							value={FirstName}
							error={FirstNameError}
							helperText={helperTextFirstName}
							label="Prénom"
							placeholder="prénom"
							variant="outlined"
							className="w-full"
							required
						/>
						<TextField
							id="outlined-basic"
							onChange={onChangeLastName}
							value={LastName}
							helperText={helperTextLastName}
							error={LastNameError}
							label="Nom"
							placeholder="nom"
							variant="outlined"
							className="w-full mt-10"
							required
						/>

						<KeyboardDatePicker
							autoOk
							variant="inline"
							inputVariant="outlined"
							label="Date de naissance"
							value={selectedDate}
							format="YYYY/MM/DD"
							minDate={"870-01-01"}
							maxDate={"3000-01-01"}
							InputAdornmentProps={{ position: 'start' }}
							onChange={onChangeDate}
							className="w-full mt-10"
						/>

						<Autocomplete
							id="combo-box-demo"
							options={countries}
							getOptionLabel={option => option.name}
							className="w-full mt-10 mb-5"
							onChange={onChangeCountryName}
							disableClearable
							disablePortal={true}
							value={{ name: countryName }}
							renderInput={params => (
								<TextField
									{...params}
									label="Pays"
									variant="outlined"
									error={errorCountry}
									helperText={helperTextCountryName}
									fullWidth
									required
								/>
							)}
						/>
						<TextField
							id="outlined-basic"
							onChange={onChangeEmail}
							label="Adresse E-mail"
							placeholder="Adresse E-mail"
							variant="outlined"
							value={email}
							className="w-full mt-10"
						/>
						<TextField
							id="outlined-basic"
							onChange={onChangeBiographie}
							label="Biographie"
							placeholder="Biographie"
							variant="outlined"
							value={biographie}
							className="w-full mt-10"
							multiline
							rows={10}
						/>

						<TextField
							id="outlined-basic"
							label="Numéro  de téléphone"
							placeholder="numéro de téléphone"
							variant="outlined"
							className="w-full mt-10"
							onChange={onChangePhoneNumber}
							value={phoneNumber}
							onKeyPress={onPress}
						/>
						<p className="mt-10 text-gray">
							<label>Image Profil</label>
							<TextField
								id="outlined-basic"
								variant="outlined"
								className="w-full mt-10"
								type="file"
								inputRef={photoRef}
								onChange={onChangePhotoPath}
							/>
						</p>
					</Box>
				</TabPanel>
			}
		/>
	);
};

export default NewAuthorWithoutAccount;
