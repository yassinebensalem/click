import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	AppBar,
	Box,
	Button,
	FormControl,
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
import { useDispatch } from 'react-redux';
import { updateAuthor, getAuthors, getActiveAuthorsByRole } from 'app/store/authorSlice';
import { unwrapResult } from '@reduxjs/toolkit';
import { getCountries } from 'app/store/countrySlice';
import { useSelector } from 'react-redux';
import CircularProgress from '@material-ui/core/CircularProgress';
import moment from 'moment';
import { decode } from 'jsonwebtoken';
const EditAuthor = ({ match }) => {
	const dispatch = useDispatch();
	const [value, setValue] = useState(0);
	const classes = useStyles();
	const [LastName, setLastName] = useState('');
	const [FirstName, setFirstName] = useState('');
	const [civilite, setCivilite] = useState('');
	const [isActive, setIsActive] = useState(false);
	const [selectedDate, setSelectedDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	const [password, setPassword] = useState('');
	const [passwordError, setPasswordError] = useState(false);
	const [helperTextPassword, setHelperTextPassword] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');
	const [confirmPasswordError, setConfirmPasswordError] = useState(false);
	const [helperTextConfirmPassword, setHelperTextConfirmPassword] =
		useState('');
	const [NewPassword, setNewPassword] = useState('');
	const [NewPasswordError, setNewPasswordError] = useState(false);
	const [helperTextNewPassword, setHelperTextNewPassword] = useState('');
	const [address, setAddress] = useState('');
	const [email, setEmail] = useState('');
	const [photoPath, setPhotoPath] = useState('');
	const [photoUpdated, setPhotoUpdated] = useState(null);
	const [authorId, setAuthorId] = useState('');
	const [countryId, setCountryId] = useState('');
	const [showPassword, setShowPassword] = useState(false);
	const [showConfirmPassword, setShowConfirmPassword] = useState(false);
	const [showNewPassword, setShowNewPassword] = useState(false);
	const [phone, setPhone] = useState('');

	const history = useHistory();
	const [enabled, setEnabled] = useState(false);
	const [disabled, setDisabled] = useState(false);
	const { countries } = useSelector(state => state.country);
	const { loadingUpdateAuthor } = useSelector(state => state.auth);
	const photoRef = useRef(null);
	const [civiliteError, setCiviliteError] = useState(false);
	const [helperTextCivilite, setHelperTextCivilite] = useState('');
	const [errorPhotoPath, setErrorPhotoPath] = useState('');
	const [helperTextPhotoPath, setHelperTextPhotoPath] = useState('');
	const {token} = useSelector(state => state.auth);
	useEffect(() => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/');
        }
		let estAffiche = true;
		dispatch(getCountries());
		dispatch(getAuthors())
			.then(unwrapResult)
			.then(res => {
				const obj = res.filter(obj => obj.id === match.params.id)[0];
				console.log(obj);
				if (obj && estAffiche) {
					setAuthorId(obj.id);
					setFirstName(obj.firstName);
					setLastName(obj.lastName);
					setEmail(obj.email);
					setAddress(obj.address);
					setCountryId(obj.countryId);
					setSelectedDate(obj.birthdate);
					setPassword(obj.password);
					setCivilite(obj.gender);
					setPhone(obj.phoneNumber);
					setAddress(obj.address);
					setPhotoPath(obj.photoPath);
					setIsActive(obj.isActive);
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
	};
	const validiteGenderError = civilite => {
		if (civilite === '') {

			setCiviliteError(true);
			setHelperTextCivilite('vous devez choisir un genre');
			//setEnabled(false);
		} else {
			//setEnabled(true);
			setCiviliteError(false);
		}
	};

	const editAuthor = () => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		let formData = new FormData();
		formData.append('Id', authorId);
		formData.append('FirstName', FirstName);
		formData.append('LastName', LastName);
		formData.append('NewPassword', NewPassword);
		formData.append('Password', password);
		formData.append('ConfirmPassword', confirmPassword);
		formData.append('CountryId', getCountryId(getCountryName()));
		formData.append('PhotoFile', photoUpdated);
		formData.append('Email', email);
		formData.append('Birthdate', selectedDate);
		formData.append('PhoneNumber', phone);
		formData.append('Gender', civilite);
		formData.append('Address', address);
		formData.append('isActive', isActive);
		validiteGenderError(getCiviliteValue());	
		dispatch(updateAuthor(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				swal({
					title: 'Auteur modifié!',
					icon: 'success'
				});
				setNewPasswordError(false);
				setHelperTextNewPassword(false);
				setConfirmPasswordError(false);
				setHelperTextConfirmPassword(false);
				
			})
			.catch(rejectedValueOrSerializedError => {
				// handle result here
				validateCurrentPassword(password, NewPassword, confirmPassword);
				

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
		setEnabled(true);

	};
	const onChangeLastName = e => {
		setLastName(e.target.value);
		setEnabled(true);

	};
	const onChangeCivilite = e => {
		if (e.target.value === 'Mr') {
			setCivilite('m');
		} else if (e.target.value === 'Mme') {
			setCivilite('f');
		} else {
			setCivilite(e.target.value);
		}
      setEnabled(true);

	};

	const onChangeDate = (e, row) => {
		setSelectedDate(row);
		setEnabled(true);


	};
	const onChangePhotoPath = e => {
		const fileObj = e.target.files && e.target.files[0];
		if (!fileObj) {
			return;
		}
		if(e.target.files[0].size/1024/1024 > 40) {
			setErrorPhotoPath(true);
			setHelperTextPhotoPath('la taille du fichier est très grande');
		}else {
			setPhotoUpdated(e.target.files[0]);
		}
		


	};
	const validateCurrentPassword = (currentPassword, password, newPassword) => {
		if (currentPassword !== '') {
			setPasswordError(true);
			setHelperTextPassword('mot de passe incorrect');

		}
	};

	
	const onChangeCountryId = (e, value) => {
		setCountryId(value.id);

	};

	


	const onChangeEmail = e => {
		setEmail(e.target.value);
		if (e.target.value.length > 0) {
			setEnabled(true);
		} else {
			setEnabled(false);
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
	const handleClickShowPassword = () => {
		setShowPassword(!showPassword);
	};


	const onChangePhone = e => {
		setPhone(e.target.value);
		if (e.target.value.length > 0) {
			setEnabled(true);
		} else {
			setEnabled(false);
		}
	};
	const onChangeAddress = e => {
		setAddress(e.target.value);
		setEnabled(true);


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
								to="/author/list"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b className="hidden sm:flex mx-4 font-medium">
									Auteurs
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
									Détails de l'auteur
								</span>
							</div>
						</div>
					</div>
					{
                      loadingUpdateAuthor ?
					  <CircularProgress />
					  :
					  <Button
						className="save-btn"
						variant="contained"
						color="primary"
						size="small"
						onClick={editAuthor}
						disabled={enabled ? false : true}
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
					<Box component="form">
						<TextField
							id="outlined-basic"
							label="Prénom"
							value={FirstName}
							defaultValue=""
							placeholder="Prénom"
							variant="outlined"
							className="w-full"
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
							minDate={"870-01-01"}
							maxDate={"3000-01-01"}
							format="YYYY/MM/DD"
							InputAdornmentProps={{ position: 'start' }}
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
							defaultValue="Email"
							disabled
							required
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
							label="Numéro de téléphone"
							variant="outlined"
							className="w-full mt-10"
							value={phone}
							onChange={onChangePhone}
							onKeyPress={onPress}
						/>
						<Autocomplete
							id="combo-box-demo"
							options={countries}
							getOptionLabel={option => option.name}
							value={{ name: getCountryName() }}
							disablePortal={true}
							onChange={onChangeCountryId}
							className="w-full mt-10"
							disableClearable
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

						<FormControl className="w-full mt-10" required>
							<InputLabel id="demo-simple-select-label">
								Civilité
							</InputLabel>
							<Select
								labelId="demo-simple-select-label"
								id="demo-simple-select"
								label="sélectionner la civilité"
								value={getCiviliteValue()}
								onChange={onChangeCivilite}
								error={civiliteError}
								helperText={helperTextCivilite}
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

export default EditAuthor;
