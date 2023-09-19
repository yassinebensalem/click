import FusePageCarded from '@fuse/core/FusePageCarded';
import { AppBar, Box, Button, Tab, Tabs, TextField } from '@material-ui/core';
import React, { useState, useEffect, useRef } from 'react';
import { Link, useHistory } from 'react-router-dom';
import ArrowBack from '@material-ui/icons/ArrowBack';
import a11yProps from '../a11props';
import { useStyles } from '@material-ui/pickers/views/Calendar/SlideTransition';
import TabPanel from '../shared-components/TabPanel';
import { KeyboardDatePicker } from '@material-ui/pickers';
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import swal from 'sweetalert';
import { useDispatch } from 'react-redux';
import {
	updateAuthorWithoutAccount,
	getAuthorsWithoutAccount
} from 'app/store/authorWithoutAccountSlice';
import { unwrapResult } from '@reduxjs/toolkit';
import { getCountries } from 'app/store/countrySlice';
import { useSelector } from 'react-redux';
import moment from 'moment';
import { decode } from 'jsonwebtoken';
const EditAuthorWithoutAccount = ({ match }) => {
	const dispatch = useDispatch();
	const [author, setAuthor] = useState(null);
	const [value, setValue] = useState(0);
	const classes = useStyles();
	const [LastName, setLastName] = useState('');
	const [FirstName, setFirstName] = useState('');

	const [selectedDate, setSelectedDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);

	const [email, setEmail] = useState('');
	const [countryName, setCountryName] = useState('');
	const [editorId, setEditorId] = useState('');
	const [countryId, setCountryId] = useState('');
	const [biographie, setBiographie] = useState('');
	const [phone, setPhone] = useState('');
	const [currentCountryId, setCurrentCountryId] = useState('');
	const [photoPath, setPhotoPath] = useState('');
	const [photoUpdated, setPhotoUpdated] = useState(null);
	const [enabled, setEnabled] = useState(false);
	const [changeCountryId, setChangeCountryId] = useState(false);
	const { countries } = useSelector(state => state.country);
	const {token} = useSelector(state => state.auth);
	const photoRef = useRef(null);
	const history = useHistory();
	useEffect(() => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		dispatch(getCountries());
		dispatch(getAuthorsWithoutAccount())
			.then(unwrapResult)
			.then(res => {
				const obj = res.filter(obj => obj.id === match.params.id)[0];

				if (obj) {
					setEditorId(obj.id);
					setFirstName(obj.firstName);
					setLastName(obj.lastName);
					setEmail(obj.email);
					setCurrentCountryId(obj.countryId);
					setSelectedDate(obj.birthdate);
					setPhone(obj.phoneNumber);
					setBiographie(obj.biography);
					setPhotoPath(obj.photoPath);
				}
				if (changeCountryId === true) {
					setEnabled(true);
				}

			});
	}, [match.params.id, changeCountryId]);
	const getCountryId = name => {
		for (let i = 0; i < countries.length; i++) {
			if (countries[i].name === name) {
				return countries[i].id;
			}
		}
	};

	const onChangeBiographie = e => {
		setBiographie(e.target.value);
		if (e.target.value.length > 0) {
			setEnabled(true);
		} else {
			setEnabled(false);
		}
	};



	const editAuthor = () => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		let formData = new FormData();
		formData.append('Id', editorId);
		formData.append('FirstName', FirstName);
		formData.append('LastName', LastName);
		formData.append('CountryId', getCountryId(getCountryName()));
		formData.append('PhotoFile', photoUpdated);
		formData.append('Email', email);
		formData.append('Birthdate', selectedDate);
		formData.append('PhoneNumber', phone);
		formData.append('Biography', biographie);
		dispatch(updateAuthorWithoutAccount(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				swal({
					title: 'Auteur sans compte modifié!',
					icon: 'success'
				});
			})
			.catch(rejectedValueOrSerializedError => {
			});
	};


	const getCountryName = () => {
		let obj = null;
		if (changeCountryId === true) {
			obj = countries.filter(country => country.id === countryId)[0];
		} else {
			obj = countries.filter(country => country.id === currentCountryId)[0];
		}

		if (obj !== undefined) {
			return obj.name;
		}
	};

	const onChangeFirstName = e => {
		setFirstName(e.target.value);
		setEnabled(true);
		if (e.target.value.length > 0) {
			setEnabled(true);
		} else {
			setEnabled(false);
		}
	};

	const onChangeDate = (e, row) => {
		setSelectedDate(row);
	};
	const onChangePhotoPath = e => {
		const fileObj = e.target.files && e.target.files[0];
		if (!fileObj) {
			return;
		}
		setEnabled(true);
		setPhotoUpdated(e.target.files[0]);
	};



	const onChangeEmail = e => {
		setEmail(e.target.value);
		if (e.target.value.length > 0) {
			setEnabled(true);
		} else {
			setEnabled(false);
		}
	};

	const onChangeLastName = e => {
		setLastName(e.target.value);
		if (e.target.value.length > 0) {
			setEnabled(true);
		} else {
			setEnabled(false);
		}
	};

	const onChangePhone = e => {
		setPhone(e.target.value);
		if (e.target.value.length > 0) {
			setEnabled(true);
		} else {
			setEnabled(false);
		}
	};

	

	const onPress = evt => {
		if (
			(evt.which != 8 && evt.which != 0 && evt.which < 48) ||
			evt.which > 57
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
								to="/authorWithoutAccount/list"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b className="hidden sm:flex mx-4 font-medium">
									Auteurs sans compte
								</b>
							</Link>
						</div>
						<div className="flex items-center max-w-full">
							<img
								src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${photoPath}`}
								alt="user"
								className="w-32 sm:w-48 rounded mr-5"
								width={40}
								height={40}
							/>
							<div className="flex flex-col mx-8">
								<h2 className='MuiTypography-root MuiTypography-body1 text-16 sm:text-20 truncate font-semibold muiltr-ehddlr"'>
									{`${LastName} ${FirstName}`}
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
							label="Nom"
							placeholder="Nom"
							value={LastName}
							variant="outlined"
							className="w-full field-admin"
							onChange={onChangeLastName}
						/>
						<TextField
							id="outlined-basic"
							label="Prénom"
							value={FirstName}
							defaultValue=""
							placeholder="Prénom"
							variant="outlined"
							className="w-full mt-10"
							onChange={onChangeFirstName}
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
						/>

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
							onChange={onChangeEmail}
							value={email}
							label="Adresse E-mail"
							variant="outlined"
							className="w-full mt-10"
							defaultValue="Email"
							disabled
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
						
							onChange={(event, value) => {
								setCountryId(value.id);
								setChangeCountryId(true);
							}}

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

					</Box>
				</TabPanel>
			}
		/>
	);
};

export default EditAuthorWithoutAccount;
