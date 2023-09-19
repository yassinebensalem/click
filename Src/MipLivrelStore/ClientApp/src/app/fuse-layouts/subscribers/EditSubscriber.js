import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	AppBar,
	Box,
	Button,
	CircularProgress,
	FormControl,
	IconButton,
	InputAdornment,
	InputLabel,
	MenuItem,
	Select,
	Tab,
	Tabs,
	TextField
} from '@material-ui/core';
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
import { Visibility, VisibilityOff } from '@material-ui/icons';
import { useDispatch } from 'react-redux';
import { updateSubscriber, getSubscribers } from 'app/store/subscriberSlice';
import { unwrapResult } from '@reduxjs/toolkit';
import { getCountries } from 'app/store/countrySlice';
import { useSelector } from 'react-redux';
import { decode } from 'jsonwebtoken';

const EditSubscriber = ({ match }) => {
	const dispatch = useDispatch();
	const [author, setAuthor] = useState(null);
	const [value, setValue] = useState(0);
	const classes = useStyles();
	const [LastName, setLastName] = useState('');
	const [FirstName, setFirstName] = useState('');
	const [civilite, setCivilite] = useState('');
	const [selectedDate, setSelectedDate] = useState(new Date());
	const [password, setPassword] = useState('');
	const [passwordError, setPasswordError] = useState('');
	const [helperTextPassword, setHelperTextPassword] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');
	const [confirmPasswordError, setConfirmPasswordError] = useState('');
	const [helperTextConfirmPassword, setHelperTextConfirmPassword] =
		useState('');
	const [NewPassword, setNewPassword] = useState('');
	const [NewPasswordError, setNewPasswordError] = useState('');
	const [helperTextNewPassword, setHelperTextNewPassword] = useState('');
	const [address, setAddress] = useState('');
	const [email, setEmail] = useState('');
	const [photoPath, setPhotoPath] = useState('');
	const [countryName, setCountryName] = useState('');
	const [countryId, setCountryId] = useState('');
	const [passwordDisabled, setPasswordDisabled] = useState(true);
	const [phone, setPhone] = useState('');
	const [subscriberId, setSubscriberId] = useState('');
	const { countries } = useSelector(state => state.country);
	const [enabled, setEnabled] = useState(false);
	const photoRef = useRef(null);
	let regexCapitalPass = /(?=.*[A-Z])/;
	let regexDigitalPass = /(?=.*\d)/;
	let regexAlphaNumericPass = /(?=.*\W)/;
	const [photoUpdated, setPhotoUpdated] = useState('');
	const {loadingUpdateEditor} = useSelector(state => state.editor);
	const {token} = useSelector(state => state.auth);
	const {history} = useHistory();
	useEffect(() => {
		if (decode(token).exp * 1000 < Date.now()) {
			history.push('/')
         }
		let estAffiche = true;
		dispatch(getCountries());
		dispatch(getSubscribers())
			.then(unwrapResult)
			.then(res => {
				const obj = res.filter(obj => obj.id === match.params.id)[0];
				if (obj && estAffiche) {
					console.log(obj);
					setFirstName(obj.firstName);
					setLastName(obj.lastName);
					setEmail(obj.email);
					setAddress(obj.address);
                    setCountryId(obj.countryId > 0 ? obj.countryId:0);
					setCivilite(obj.gender);
					
					if(obj.birthdate){
						setSelectedDate(obj.birthdate);
					}
					setPassword(obj.password);
					setSubscriberId(obj.id);
					setPhone(obj.phoneNumber);
					setPhotoPath(obj.photoPath);
				}
				return () => {
					estAffiche = false;
				};
			});
	}, [match.params.id]);
	const getCountryId = name => {
		for (let i = 0; i < countries.length; i++) {
			if (countries[i].name === name) {
				return countries[i].id;
			}
        }
        return 0;
	};
	const EditSubscriber = () => {
		let formData = new FormData();
		formData.append('Id', subscriberId);
		formData.append('FirstName', FirstName);
		formData.append('LastName', LastName);
		formData.append('NewPassword', NewPassword);
		formData.append('Password', password);
		formData.append('ConfirmPassword', confirmPassword);
		formData.append('CountryId', getCountryId(getCountryName()));
		if (photoRef.current.value !== null) {
			formData.append('PhotoFile', photoUpdated);
		}
		formData.append('Email', email);
		formData.append('Birthdate', selectedDate);
		formData.append('PhoneNumber', phone);
		formData.append('Gender', civilite);
		formData.append('Address', address);

		dispatch(updateSubscriber(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				swal({
					title: 'Client modifié!',
					icon: 'success'
				});
				setNewPasswordError(false);
				setHelperTextNewPassword(false);
				setConfirmPasswordError(false);
				setHelperTextConfirmPassword(false);
			})
			.catch(rejectedValueOrSerializedError => {
				// handle result here
				validateCurrentPassword(password);
				setPasswordError(true);
			});
	};
	const getCountryName = () => {
		const obj = countries.filter(country => country.id === countryId)[0];
		if (obj !== undefined) {
			return obj.name;
		}
	};

	const onChangeFirstName = e => {
		setFirstName(e.target.value);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};
	const onChangeCivilite = e => {
		if (e.target.value === 'Mr') {
			setCivilite('m');
		} else if (e.target.value === 'Mme') {
			setCivilite('f');
		} else {
			setCivilite(e.target.value);
		}
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};

	const onChangeDate = (e, row) => {
		setSelectedDate(row);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};
	const onChangePhotoPath = e => {
		const fileObj = e.target.files && e.target.files[0];
		if (!fileObj) {
			return;
		}
		setPhotoUpdated(e.target.files[0]);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};
	const validateCurrentPassword = currentPassword => {
		if (currentPassword !== '') {
			setPasswordError(true);
			setHelperTextPassword("Ce mot de passe est incorrect.");
		}
	};
	const onChangePassword = e => {
		setPassword(e.target.value);
		setPasswordError(false);
		setHelperTextPassword('');
		if (NewPassword == "" && confirmPassword === "") {
			setEnabled(false)
		} else {
			setEnabled(true)
		}
		if (e.target.value.length > 0) {
			setPasswordDisabled(false);
		} else {
			setPasswordDisabled(true);
		}
	};

	const onChangeCountryName = (e, value) => {
		setCountryName(value.name);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};
	const validateNewPasswordError = (password, newPassword) => {
		if (password !== '') {
			if (newPassword === '') {
				setNewPasswordError(false);
				setHelperTextNewPassword('');
				setEnabled(true);
			} else if (newPassword.trim().length < 6) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'votre mot de passe doit comporter au moins 6 et au maximum 25 caractères.'
				);
				setEnabled(false);
			} else if (
				regexDigitalPass.test(newPassword) === false &&
				regexCapitalPass.test(newPassword) === false
			) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'le mot de passe doit avoir au moins un nombre digital et une lettre majuscule'
				);
				setEnabled(false);
			} else if (
				regexCapitalPass.test(newPassword) === false &&
				regexAlphaNumericPass.test(newPassword) === false
			) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'le mot de passe doit avoir au moins une lettre majuscule et une lettre alphanumérique'
				);
				setEnabled(false);
			} else if (
				regexDigitalPass.test(newPassword) === false &&
				regexAlphaNumericPass.test(newPassword) === false
			) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'le mot de passe doit avoir au moins un nombre digital et une lettre alphanumérique'
				);
				setEnabled(false);
			} else if (regexDigitalPass.test(newPassword) === false) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'le mot de passe doit avoir au moins un nombre digital'
				);
				setEnabled(false);
			} else if (regexCapitalPass.test(newPassword) === false) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'le mot de passe doit avoir au moins une lettre majuscule'
				);
				setEnabled(false);
			} else if (regexAlphaNumericPass.test(newPassword) === false) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'le mot de passe doit avoir au moins une lettre aplhanumérique'
				);
				setEnabled(false);
			} else if (password === newPassword) {
				setNewPasswordError(true);
				setHelperTextNewPassword(
					'vous devez entrez un nouveau mot de passe'
				);
				setEnabled(false);
			} else if (confirmPassword !== "") {
				if (password !== confirmPassword) {
					setNewPasswordError(true);
					setHelperTextNewPassword('les deux mots de passe ne sont pas identiques');
					setEnabled(false);
				} else {
					setEnabled(true)
				}
			}
		}
	};
	const onChangeNewPassword = e => {
		setNewPassword(e.target.value);
		setNewPasswordError(false);
		setHelperTextNewPassword('');
		validateNewPasswordError(password, e.target.value);
	};
	const validateConfirmPassword = (confirmPassword, NewPassword) => {
		if (confirmPassword === '') {
			setConfirmPasswordError(true);
			setHelperTextConfirmPassword(
				'vous devez confirmer votre nouveau mot de passe'
			);
			setEnabled(false);
		} else if (confirmPassword !== NewPassword) {
			setConfirmPasswordError(true);
			setHelperTextConfirmPassword(
				'les deux mots de passe ne sont pas identiques'
			);
			setHelperTextNewPassword('');
			setEnabled(false);
		} else {
			setEnabled(true);
		}
	};
	const onChangeConfirmPassword = e => {
		setConfirmPassword(e.target.value);
		setConfirmPasswordError(false);
		setHelperTextConfirmPassword('');
		validateConfirmPassword(e.target.value, NewPassword);
		if (password === NewPassword) {
			setEnabled(false);
		}
	};

	const onChangeEmail = e => {
		setEmail(e.target.value);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};
	const onChangeAddress = e => {
		setAddress(e.target.value);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};

	const onChangeLastName = e => {
		setLastName(e.target.value);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
	};
	const getCiviliteValue = () => {
		if (civilite === 'm') {
			return 'Mr';
		} else if (civilite === 'f') {
			return 'Mme';
		} else {
			return civilite;
		}
	};
	

	const handleChangeTab = (e, newValue) => {
		setValue(newValue);
	};

	const onChangePhone = e => {
		setPhone(e.target.value);
		if (password === undefined) {
			setEnabled(true);
		} else {
			setEnabled(false)
		}
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
								src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${photoPath}`}
								alt="user"
								className="w-32 sm:w-48 rounded"
								width={40}
								height={40}
							/>
							<div className="flex flex-col mx-8">
								<h2 className='MuiTypography-root MuiTypography-body1 text-16 sm:text-20 truncate font-semibold muiltr-ehddlr"'>
									{`${FirstName} ${LastName}`}
								</h2>{' '}
								<br />
								<span
									style={{
										marginTop: '-10px',
										fontSize: '12px'
									}}
								>
									Détails du client
								</span>
							</div>
						</div>
					</div>
					{
						loadingUpdateEditor ?
						<CircularProgress color='#F75454' size={20} />
						:
						<Button
						onClick={EditSubscriber}
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
								label="Prénom"
								value={FirstName}
								defaultValue=""
								placeholder="Prénom"
								variant="outlined"
								className="w-full "
								onChange={onChangeFirstName}
							/>
							<TextField
								id="outlined-basic"
								label="Nom"
								placeholder="Nom"
								value={LastName}
								variant="outlined"
								className="w-full field-admin mt-10"
								onChange={onChangeLastName}
							/>

							<KeyboardDatePicker
								autoOk
								variant="inline"
								inputVariant="outlined"
								label="Date de naissance"
								value={selectedDate}
								format="YYYY/MM/DD"
								InputAdornmentProps={{ position: 'start' }}
								minDate={"870-01-01"}
							    maxDate={"3000-01-01"}
								onChange={onChangeDate}
								className="w-full mt-10"
							/>

							<TextField
								id="outlined-basic"
								onChange={onChangeEmail}
								value={email}
								label="Adresse E-mail"
								variant="outlined"
								className="w-full mt-10"
								defaultValue="E-mail"
								required
								disabled
							/>
							<TextField
								id="outlined-basic"
								label="Numéro de téléphone"
								variant="outlined"
								className="w-full mt-10"
								onChange={onChangePhone}
								value={phone}
								onKeyPress={onPress}
							/>
							<Autocomplete
								id="combo-box-demo"
								options={countries}
								getOptionLabel={option => option.name}
								value={{ name: getCountryName() }}
								disablePortal={true}
								onChange={(event, value) =>
									setCountryId(value.id)
								}
								className="w-full mt-10"
								disableClearable
								renderInput={params => (
									<TextField
										{...params}
										label="Pays"
										variant="outlined"
										fullWidth
									/>
								)}
							/>
							<FormControl
								className="w-full mt-10"
								required
							>
								<InputLabel id="demo-simple-select-label">
									Sélectionner la civilité
								</InputLabel>
								<Select
									labelId="demo-simple-select-label"
									id="demo-simple-select"
									label="sélectionner la civilité"
									value={getCiviliteValue()}
									onChange={onChangeCivilite}
								>
									<MenuItem value="Mme">Mme</MenuItem>
									<MenuItem value="Mr">Mr</MenuItem>
								</Select>
							</FormControl>
							<p className="mt-10 text-gray">
								<label>Image Profil</label>
								<TextField
									id="outlined-basic"
									variant="outlined"
									className="w-full mt-10"
									type="file"
									onChange={onChangePhotoPath}
									inputRef={photoRef}
								/>
							</p>
							<TextField
								id="outlined-basic"
								name="address"
								variant="outlined"
								className="w-full mt-10"
								onChange={onChangeAddress}
								label="Adresse"

								value={address}
							/>
						</Box>
					</TabPanel>
				</>
			}
		/>
	);
};

export default EditSubscriber;
