import FusePageCarded from '@fuse/core/FusePageCarded';
import React, { useState, useEffect, useRef } from 'react';
import {
	AppBar,
	Box,
	Button,
	FormControl,
	FormHelperText,
	IconButton,
	InputAdornment,
	InputLabel,
	makeStyles,
	MenuItem,
	Select,
	Snackbar,
	Tab,
	Tabs,
	TextField,
	withStyles
} from '@material-ui/core';
import CircularProgress from '@material-ui/core/CircularProgress';
import { unwrapResult } from '@reduxjs/toolkit';

import { Link, useHistory } from 'react-router-dom';
import ArrowBack from '@material-ui/icons/ArrowBack';
import a11yProps from '../a11props';
import user from '../../../images/profile.jpg';
import TabPanel from '../shared-components/TabPanel';
import { KeyboardDatePicker } from '@material-ui/pickers';
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import swal from 'sweetalert';
import moment from 'moment';

import { useDispatch, useSelector } from 'react-redux';
import { createAuthor, getAuthors } from 'app/store/authorSlice';
import { getCountries } from 'app/store/countrySlice';
import { alpha, styled } from '@material-ui/styles';
import { decode } from 'jsonwebtoken';
import { logout } from 'app/store/authSlice';
const useStyles = makeStyles({
	'& label.Mui-focused': {
		color: 'green',
	},
	'& .MuiInput-underline:after': {
		borderBottomColor: 'green',
	},
	'& .MuiOutlinedInput-root': {
		'& fieldset': {
			borderColor: 'red',
		},
		'&:hover fieldset': {
			borderColor: 'yellow',
		},
		'&.Mui-focused fieldset': {
			borderColor: 'green',
		},
	},
});
const CssTextField = styled(TextField)({
	'& label.Mui-focused': {
		color: 'red',
	},

	'& .MuiOutlinedInput-root': {
		'& fieldset:focus': {
			borderColor: 'red',
		},
		'&:focus fieldset': {
			borderColor: 'red',
		},
		'&.Mui-focused fieldset': {
			borderColor: 'red',
		},
	},
});
const NewAuthor = () => {
	let regex =
		/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

	const [value, setValue] = useState(0);

	const classes = useStyles();
	const [LastName, setLastName] = useState('');
	const [LastNameError, setLastNameError] = useState(false);
	const [helperTextLastName, setHelperTextLastName] = useState('');
	const [FirstName, setFirstName] = useState('');
	const [FirstNameError, setFirstNameError] = useState(false);
	const [helperTextFirstName, setHelperTextFirstName] = useState('');
	const [civiliteError, setCiviliteError] = useState('');
	const [helperTextCivilite, setHelperTextCivilite] = useState('');
	const [selectedDate, setSelectedDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	
	const [address, setAddress] = useState('');
	const [email, setEmail] = useState('');
	const [emailError, setEmailError] = useState('');
	const [helperTextEmail, setHelperTextEmail] = useState('');
	const [photoPath, setPhotoPath] = useState('');
	const [errorPhotoPath, setErrorPhotoPath] = useState('');
	const [helperTextPhotoPath, setHelperTextPhotoPath] = useState('');
	const [countryName, setCountryName] = useState('');
	const [helperTextCountryName, setHelperTextCountryName] = useState(null);
	const [errorCountry, setErrorCountry] = useState(false);


	const [biographie, setBigoraphie] = useState('');
	const [phoneNumber, setPhoneNumber] = useState('');
	const [countryId, setCountryId] = useState('');
	const dispatch = useDispatch();
	const { authors, loadingSaveAuthor } = useSelector(state => state.author);
	const { countries } = useSelector(state => state.country);
	const [civilite, setCivilite] = useState('');
	const {token} = useSelector(state => state.auth);
	const photoRef = useRef(null);
	const history = useHistory();

	const onChangeLastName = e => {
		setLastName(e.target.value);
		setLastNameError(false);
		setHelperTextLastName('');
	};

	useEffect(() => {
		dispatch(getCountries());
		dispatch(getAuthors());
		
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
		    history.push('/');
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
		const arr = authors.filter(author => author.email === email);
		if (arr.length > 0) {
			bool = true;
		} else {
			bool = false;
		}

		return bool;
	};

	const findFirstName = firstName => {
		let bool;
		const arr = authors.filter(author => author.firstName === firstName);
		if (arr.length > 0) {
			bool = true;
		} else {
			bool = false;
		}

		return bool;
	}
	const findLastName = lastName => {
		let bool;
		const arr = authors.filter(author => author.lastName === lastName);
		if (arr.length > 0) {
			bool = true;
		} else {
			bool = false;
		}

		return bool;
	}

	const getCountryName = () => {
		const obj = countries.filter(country => country.id === countryId)[0];
		if (obj !== undefined) {
			return obj.name;
		}
	};

	const onChangeFirstName = e => {
		setFirstName(e.target.value);
		setFirstNameError(false);
		setHelperTextFirstName('');
	};

	const onChangeDate = (e, row) => {
		setSelectedDate(row);
	};
	const onChangePhotoPath = e => {
		if(e.target.files[0].size/1024/1024 > 40) {
			setErrorPhotoPath(true);
			setHelperTextPhotoPath('la taille du fichier est très grande');
			photoRef.current.value = null;
		}else {
			setPhotoPath(e.target.files[0]);
		}
	
	};



	const validateCountryError = id => {
			setErrorCountry(true);
			setHelperTextCountryName('vous devez choisir un pays');
	};
	const onChangeCountryName = (e, value) => {
		setCountryName(value.name);
		setErrorCountry(false);
		setHelperTextCountryName('');
	};

	const onChangePhoneNumber = e => {
		setPhoneNumber(e.target.value);
	};
	const validateEmail = email => {
		if (regex.test(email) === false) {
			setEmailError(true);
			setHelperTextEmail('Vous devez entrer un e-mail valide');
		} else if (findEmail(email) === true) {
			setEmailError(true);
			setHelperTextEmail('email existant');
		}
	};
	const onChangeEmail = e => {
		setEmail(e.target.value);
		setEmailError(false);
		setHelperTextEmail('');
		validateEmail(e.target.value.trim());
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
	}
   };
	const validiteGenderError = civilite => {
		if (civilite === '') {
			setCiviliteError(true);
			setHelperTextCivilite('vous devez choisir un genre');
		} 
	};
	const onChangeCivilite = e => {
		setCivilite(e.target.value);
		setCiviliteError(false);
		setHelperTextCivilite('');
		validiteGenderError(e.target.value);
	};

	const getCiviliteValue = () => {
		if (civilite === 'Mr') {
			return 'm';
		} else if (civilite === 'Mme') {
			return 'f';
		} else {
			return civilite;
		}
	};

	const onChangeAddress = e => {
		setAddress(e.target.value);
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

	const reset = () => {
		setFirstName('');
		setLastName('');
		setEmail('');
		setCountryName('');
		setBigoraphie('');
		setPhoneNumber('');
		setCivilite('');
		setSelectedDate(moment(new Date()).format('YYYY-MM-DD'));
		setAddress('');
		photoRef.current.value = null;
	};
	console.log(LastNameError);
	const onAddAuthor = () => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
		    history.push('/');
        }
		const formData = new FormData();
		formData.append('LastName', LastName);
		formData.append('FirstName', FirstName);
		formData.append('Birthdate', selectedDate);
		formData.append('Email', email);
		formData.append('Gender', getCiviliteValue());
		formData.append('Address', address);
		formData.append('CountryId', getCountryId(countryName));
		formData.append('PhotoFile', photoPath);
		formData.append('PhoneNumber', phoneNumber);
		formData.append('UserRole', 3);
		validateEmail(email);
		validiteGenderError(civilite);
		validateFirstName(FirstName);
		validateLastName(LastName);
        if (getCountryId(countryName) == undefined) {
			validateCountryError(getCountryId(countryName));
		}

		if(FirstName!=="" && LastName!=="") {
			dispatch(createAuthor(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				// handle result here
	
				swal({
					title: 'Auteur ajouté!',
					icon: 'success'
				});
				reset();
			})
			.catch(rejectedValueOrSerializedError => { }); 
		}
		  
} 
	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center">
					<div className="pt-10 pb-10">
						<div className="flex flex-col max-w-full min-w-0">
								<Link
										to="/author/list"
										className="flex items-center sm:mb-8"
										style={{
											color: 'white',
											textDecoration: 'none'
										}}
									>
										<ArrowBack fontSize="5px" />
										<b className="hidden sm:flex mx-4 font-medium">
											Auteurs sans ME
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
										: 'Nouvel Auteur'}
								</h2>
								<span className="mt-4 text-xs">
									Détails de l'auteur
								</span>
							</div>
						</div>
					</div>
					{
						loadingSaveAuthor ?
						<CircularProgress color='#F75454' size={20} />
						:
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
					}
					
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
					<Box component="form"
					>

						<TextField
							id="outlined-error"
							onChange={onChangeFirstName}
							value={FirstName}
							error={FirstNameError}
							helperText={helperTextFirstName}
							label="Prénom"
							placeholder="Prénom"
							className="w-full"
							variant="outlined"
							required
						/>
						<TextField
							id="outlined-basic"
							onChange={onChangeLastName}
							value={LastName}
							error={LastNameError}
							helperText={helperTextLastName}
							label="Nom"
							placeholder="Nom"
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
							InputAdornmentProps={{ position: 'start' }}
							onChange={onChangeDate}
							minDate={"870-01-01"}
							maxDate={"3000-01-01"}
							className="w-full mt-10"
						/>
				
						<TextField
							id="outlined-basic"
							onChange={onChangeEmail}
							label="Adresse E-mail"
							placeholder="Adresse E-mail"
							variant="outlined"
							value={email}
							className="w-full mt-10"
							required
							error={emailError}
							helperText={helperTextEmail}
						/>
						

						<TextField
							id="outlined-basic"
							onChange={onChangeAddress}
							label="Adresse"
							placeholder="Adresse"
							variant="outlined"
							value={address}
							className="w-full mt-10"
						/>
						<TextField
							id="outlined-basic"
							label="Numéro  de téléphone"
							placeholder="Numéro de téléphone"
							variant="outlined"
							className="w-full mt-10"
							onChange={onChangePhoneNumber}
							value={phoneNumber}
							onKeyPress={onPress}
						/>
	
						<Autocomplete
									id="combo-box-demo"
									options={countries}
									getOptionLabel={option => option.name}
									className="w-full mt-10 mb-5"
									onChange={onChangeCountryName}
									disableClearable
									disablePortal
									value={{ name: countryName }}
									required
									renderInput={params => (
										<TextField
											{...params}
											label="Pays"
											variant="outlined"
											fullWidth
											error={errorCountry}
											helperText={helperTextCountryName}
											required
										/>
									)}
						/>
							

						<FormControl
							className="w-full mt-10 mb-5"
							error={civiliteError}
							required
						>
							<InputLabel id="demo-simple-select-label">
								Civilité
							</InputLabel>
							<Select
								labelId="demo-simple-select-label"
								id="demo-simple-select"
								label="sélectionner la civilité"
								value={civilite}
								onChange={onChangeCivilite}
							>
								<MenuItem value="Mme">Mme</MenuItem>
								<MenuItem value="Mr">Mr</MenuItem>
							</Select>
							<FormHelperText>
								{helperTextCivilite}
							</FormHelperText>
						</FormControl>
						<p className="mt-10 text-gray">
							<label>Image Profil</label>
							<TextField
								id="outlined-basic"
								variant="outlined"
								className="w-full mt-10"
								type="file"
								inputRef={photoRef}
								onChange={onChangePhotoPath}
								error={errorPhotoPath}
								helperText={helperTextPhotoPath}
							/>
						</p>
					</Box>
				</TabPanel>
			}
		/>
	);
};

export default NewAuthor;
