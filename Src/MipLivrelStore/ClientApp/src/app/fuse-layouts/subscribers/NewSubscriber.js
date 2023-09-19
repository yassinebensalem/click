import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	AppBar,
	Box,
	Button,
	CircularProgress,
	FormControl,
	FormHelperText,
	IconButton,
	InputAdornment,
	InputLabel,
	MenuItem,
	Select,
	Snackbar,
	Tab,
	Tabs,
	TextField
} from '@material-ui/core';
import { unwrapResult } from '@reduxjs/toolkit';
import React, { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import ArrowBack from '@material-ui/icons/ArrowBack';
import a11yProps from '../a11props';
import { useStyles } from '@material-ui/pickers/views/Calendar/SlideTransition';
import user from '../../../images/profile.jpg';
import TabPanel from '../shared-components/TabPanel';
import { KeyboardDatePicker } from '@material-ui/pickers';
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import swal from 'sweetalert';

import { Visibility, VisibilityOff } from '@material-ui/icons';

import { useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { getAll } from '../../store/editorJoinSlice';
import { updateJoinRequest } from '../../store/authorJoinSlice';
import { createSubscriber, getSubscribers } from 'app/store/subscriberSlice';
import { getCountries } from 'app/store/countrySlice';
import moment from 'moment';

const NewClient = () => {
	let regex =
		/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	let regexCapitalPass = /(?=.*[A-Z])/;
	let regexDigitalPass = /(?=.*\d)/;
	let regexAlphaNumericPass = /(?=.*\W)/;
	let regexPass = /(?=.*\d)(?=.*[A-Z])(?=.*\W)/;
	const [value, setValue] = useState(0);
	const classes = useStyles();
	const [LastName, setLastName] = useState('');
	const [LastNameError, setLastNameError] = useState('');
	const [helperTextLastName, setHelperTextLastName] = useState('');
	const [FirstName, setFirstName] = useState('');
	const [FirstNameError, setFirstNameError] = useState('');
	const [helperTextFirstName, setHelperTextFirstName] = useState('');
	const [civilite, setCivilite] = useState('');
	const [civiliteError, setCiviliteError] = useState('');
	const [helperTextCivilite, setHelperTextCivilite] = useState('');
	const [selectedDate, setSelectedDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	const [password, setPassword] = useState('');
	const [passwordError, setPasswordError] = useState('');
	const [helperTextPassword, setHelperTextPassword] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');
	const [confirmPasswordError, setConfirmPasswordError] = useState('');
	const [helperTextConfirmPassword, setHelperTextConfirmPassword] =
		useState('');
	const [address, setAddress] = useState('');
	const [addressError, setAddressError] = useState('');
	const [helperTextAddress, setHelperTextAddress] = useState('');
	const [email, setEmail] = useState('');
	const [emailError, setEmailError] = useState('');
	const [helperTextEmail, setHelperTextEmail] = useState('');
	const [photoPath, setPhotoPath] = useState('');
	const [biographie, setBiographie] = useState('');

	const [countryName, setCountryName] = useState('');
	const [helperTextCountryName, setHelperTextCountryName] = useState(null);
	const [errorCountry, setErrorCountry] = useState(false);

	
	const [errorPhotoPath, setErrorPhotoPath] = useState('');
	const [helperTextPhotoPath, setHelperTextPhotoPath] = useState('');
	const [phoneNumber, setPhoneNumber] = useState('');
	const [countryId, setCountryId] = useState('');

	const { id } = useParams();
	const dispatch = useDispatch();
	const { editorsJoined, updated } = useSelector(state => state.editorJoin);
	const { editors } = useSelector(state => state.editor);
	const { countries } = useSelector(state => state.country);
	const { loadingSaveSubscriber } = useSelector(state => state.susbcriber);
	const [editorName, setEditorName] = useState('Nouvel Editeur');
	const [enabled, setEnabled] = useState(true);
	const photoRef = useRef(null);

	const onChangeLastName = e => {
		setLastName(e.target.value);
		setLastNameError(false);
		setHelperTextLastName('');
	};
	useEffect(() => {
		dispatch(getCountries());
		dispatch(getSubscribers());
	}, []);

	const findEditorById = id => {
		const editor = editorsJoined.find(editor => editor.id == id);
		return editor;
	};
	const getCountryId = name => {
		for (let i = 0; i < countries.length; i++) {
			if (countries[i].name === name) {
				return countries[i].id;
			}
		}
	};

	const findEmail = email => {
		let bool;
		const arr = editors.filter(editor => editor.email === email);

		if (arr.length > 0) {
			bool = true;
		} else {
			bool = false;
		}
		return bool;
	};

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
	const validatePasswordError = password => {
		if (password === '') {
			setPasswordError(true);
			setHelperTextPassword('champ vide');
			setEnabled(false);
		} else if (password.length < 6 || password.length > 100) {
			setPasswordError(true);
			setHelperTextPassword(
				'votre mot de passe doit comporter au moins 6 et au maximum 100 caractères.'
			);
			setEnabled(false);
		} else if (
			regexDigitalPass.test(password) === false &&
			regexCapitalPass.test(password) === false
		) {
			setPasswordError(true);
			setHelperTextPassword(
				'le mot de passe doit avoir au moins un nombre digital et une lettre majuscule'
			);
			setEnabled(false);
		} else if (
			regexCapitalPass.test(password) === false &&
			regexAlphaNumericPass.test(password) === false
		) {
			setPasswordError(true);
			setHelperTextPassword(
				'le mot de passe doit avoir au moins une lettre majuscule et une lettre alphanumérique'
			);
			setEnabled(false);
		} else if (
			regexDigitalPass.test(password) === false &&
			regexAlphaNumericPass.test(password) === false
		) {
			setPasswordError(true);
			setHelperTextPassword(
				'le mot de passe doit avoir au moins un nombre digital et une lettre alphanumérique'
			);
			setEnabled(false);
		} else if (regexDigitalPass.test(password) === false) {
			setPasswordError(true);
			setHelperTextPassword(
				'le mot de passe doit avoir au moins un nombre digital'
			);
		} else if (regexCapitalPass.test(password) === false) {
			setPasswordError(true);
			setHelperTextPassword(
				'le mot de passe doit avoir au moins une lettre majuscule'
			);
			setEnabled(false);
		} else if (regexAlphaNumericPass.test(password) === false) {
			setPasswordError(true);
			setHelperTextPassword(
				'le mot de passe doit avoir au moins une lettre aplhanumérique'
			);
			setEnabled(false);
		} else if (confirmPassword === '') {
			setEnabled(false);
		} else {
			setEnabled(true);
		}
	};
	const onChangePassword = e => {
		setPassword(e.target.value);
		setPasswordError(false);
		setHelperTextPassword('');
		validatePasswordError(e.target.value.trim());
	};
	const validateCountryError = id => {
		if (id === undefined) {
			setErrorCountry(true);
			setHelperTextCountryName('vous devez choisir un pays');
			setEnabled(false);
			console.log(id);
		} else {
			setEnabled(true);
		}
	};
	const onChangeCountryName = (e, value) => {
		setCountryName(value.name);
		setErrorCountry(false);
		setHelperTextCountryName('');
	};
	const validateConfirmPasswordError = confirm => {
		if (confirm === '') {
			setConfirmPasswordError(true);
			setHelperTextConfirmPassword('champ vide');
			setEnabled(false);
		} else if (confirm !== password) {
			setConfirmPasswordError(true);
			setHelperTextConfirmPassword(
				'les deux mots de passe doivent être identiques'
			);
			setEnabled(false);
		} else {
			setEnabled(true);
		}
	};

	const onChangeConfirmPassword = e => {
		setConfirmPassword(e.target.value);
		setConfirmPasswordError(false);
		setHelperTextConfirmPassword('');
		validateConfirmPasswordError(e.target.value.trim());
	};
	const onChangePhoneNumber = e => {
		setPhoneNumber(e.target.value);
	};
	const validateEmail = email => {
		if (regex.test(email) === false) {
			setEmailError(true);
			setHelperTextEmail('email invalide');
			setEnabled(false);
		} else if (findEmail(email) === true) {
			setEmailError(true);
			setHelperTextEmail('email existant');
			setEnabled(false);
		} else {
			setEnabled(true);
		}
	};
	const onChangeEmail = e => {
		setEmail(e.target.value);
		setEmailError(false);
		setHelperTextEmail('');
		validateEmail(e.target.value.trim());
	};
	const onChangeAddress = e => {
		setAddress(e.target.value);
		setAddressError(false);
		setHelperTextAddress('');
	};
	const validiteGenderError = civilite => {
		if (civilite === '') {
			setCiviliteError(true);
			setHelperTextCivilite('vous devez choisir un genre');
			setEnabled(false);
		} else {
			setEnabled(true);
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


	const reset = () => {
		setFirstName('');
		setLastName('');
		setEmail('');
		setPassword('');
		setConfirmPassword('');
		setCountryName('');
		setPhoneNumber('');
		setCivilite('');
		setSelectedDate(moment(new Date()).format('YYYY-MM-DD'));
		setAddress('');
		photoRef.current.value = null;
		setEnabled(false);
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

	const onAddSubscriber = () => {
		const formData = new FormData();
		formData.append('LastName', LastName);
		formData.append('FirstName', FirstName);
		formData.append('Birthdate', selectedDate);
		formData.append('Email', email);
		formData.append('Password', password);
		formData.append('ConfirmPassword', confirmPassword);
		formData.append('Gender', getCiviliteValue());
		formData.append('Address', address);
		formData.append('CountryId', getCountryId(countryName));
		formData.append('PhotoFile', photoPath);
		formData.append('phoneNumber', phoneNumber);
		formData.append('UserRole', 2);

		validatePasswordError(password);
		validateConfirmPasswordError(confirmPassword);
		validateEmail(email);
		validiteGenderError(civilite);
		validateCountryError(getCountryId(countryName));

		dispatch(createSubscriber(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				// handle result here
				swal({
					title: 'Client ajouté!',
					icon: 'success'
				});
				reset();
			})
			.catch(rejectedValueOrSerializedError => {
				console.log(rejectedValueOrSerializedError);
			});
	};
	const handleChangeTab = (e, newValue) => {
		setValue(newValue);
	};
	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center">
					<div className="pt-10 pb-10">
						<div className="flex flex-col max-w-full min-w-0">
							<Link
								to="/subscriber/list"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b className="hidden sm:flex mx-4 font-medium">
									Clients
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
										: 'Nouveau Client'}
								</h2>{' '}
								<br />
								<span
									style={{
										marginTop: '-10px',
										fontSize: '12px'
									}}
								>
									Détails de client
								</span>
							</div>
						</div>
					</div>
					{
						loadingSaveSubscriber ?
						<CircularProgress color='#F75454' size={20} />
						:
						<Button
						onClick={onAddSubscriber}
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
							onChange={handleChangeTab}
						>
							<Tab label="Infos Basiques" {...a11yProps(0)} />
						</Tabs>
					</AppBar>
				</div>
			}
			content={
				<>
					<TabPanel value={value} index={0}>
						<Box component="form">
							<TextField
								id="outlined-basic"
								onChange={onChangeFirstName}
								value={FirstName}
								label="Prénom"
								placeholder="Prénom"
								variant="outlined"
								className="w-full"
							/>

							<TextField
								id="outlined-basic"
								onChange={onChangeLastName}
								value={LastName}
								label="Nom"
								placeholder="Nom"
								variant="outlined"
								className="w-full mt-10"
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
								className="w-full mt-10"
								minDate={"870-01-01"}
						    	maxDate={"3000-01-01"}
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
								label="Numéro  de téléphone"
								placeholder="Numéro de téléphone"
								variant="outlined"
								className="w-full mt-10"
								onChange={onChangePhoneNumber}
								value={phoneNumber}
								onKeyPress={onPress}
							/>
							{id ? (
								<Autocomplete
									id="combo-box-demo"
									options={countries}
									getOptionLabel={option => option.name}
									className="w-full mt-10"
									onChange={(event, value) =>
										setCountryId(value.id)
									}
									name="country"
									noOptionsText="pas de pays"
									value={{ name: getCountryName() }}
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
							) : (
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
												fullWidth
												required
											/>
										)}
									/>
								)}

							<FormControl
								className="w-full mt-10"
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
									onChange={onChangePhotoPath}
									type="file"
									inputRef={photoRef}
									error={errorPhotoPath}
									helperText={helperTextPhotoPath}
								/>
							</p>
							<TextField
								id="outlined-basic"
								placeholder="Adresse"
								label="Adresse"
								name="Address"
								variant="outlined"
								className="w-full mt-10"
								onChange={onChangeAddress}
								value={address}
							/>
							
						</Box>
					</TabPanel>
				</>
			}
		/>
	);
};

export default NewClient;
