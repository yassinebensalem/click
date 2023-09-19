import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	FormControlLabel,
	AppBar,
	Box,
	makeStyles,
	TextField,
	Typography,
	Texta,
	TextareaAutosize,
	Checkbox
} from '@material-ui/core';
import ArrowBack from '@material-ui/icons/ArrowBack';
import EmojyEvents from '@material-ui/icons/EmojiEvents';
import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import styled from 'styled-components';
import book from '../../../images/book.png';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Button from '@material-ui/core/Button/Button';
import TabPanel from '../shared-components/TabPanel';
import a11yProps from '../a11props';
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import { KeyboardDatePicker } from '@material-ui/pickers';
import { Link } from 'react-router-dom';
import { getBooks } from 'app/store/bookSlice';
import {
	addNewPrize,
	getBookByTitle,
	reset,
	addPrizeBook,
	getAllPrizedBooks,
	getPrizes
} from 'app/store/prizeSlice';
import { getCountries } from 'app/store/countrySlice';
import { unwrapResult } from '@reduxjs/toolkit';
import swal from 'sweetalert';
import { load } from 'jsdom/lib/jsdom/browser/resource-loader';
import { DataGrid } from '@material-ui/data-grid';
import PrizeRow from '../shared-components/PrizeRow';
import NoRowsOverlay from './NoRowsOverlay';

const useStyles = makeStyles(theme => ({
	hideRightSeparator: {
		'& > .MuiDataGrid-columnSeparator': {
			visibility: 'hidden'
		},
		'& .MuiDataGrid-colCellTitle': {
			whiteSpace: 'initial'
		}
	},
	root: {
		'& .MuiDataGrid-mainGridContainer': {
			borderTopLeftRadius: '20px',
			borderTopRightRadius: '20px'
		}
	}
}));

const NewPrize = () => {
	const { countries } = useSelector(state => state.country);
	const { books } = useSelector(state => state.book);
	const { prizedBooks, booksByTitle, prizes } = useSelector(
		state => state.prize
	);
	const [selectedDate, setSelectedDate] = useState(new Date());
	const [formData, setFormData] = useState({
		Title: '',
		CoverPath: '',
		Price: 0,
		Description: '',
		PageNumbers: 0,
		PublicationDate: '',
		ISBN: '',
		ISSN: '',
		EISBN: '',
		PDFPath: '',
		AuthorId: '',
		PublisherId: '',
		CategoryId: '',
		CountryId: '',
		LanguageId: ''
	});

	const [successMsg, setSuccessMsg] = useState('');
	const [open, setOpen] = useState(false);
	const [BookId, setBookId] = useState(null);
	const [PrizeId, setPrizeId] = useState('');
	const [PrizeName, setPrizeName] = useState('');
	const [disabled, setDisabled] = useState(true);
	const dispatch = useDispatch();

	const handleChange = e => {
		setFormData({
			...formData,
			[e.target.name]: e.target.value
		});
	};

	const [value, setValue] = useState(0);
	const [values, setValues] = useState([]);
	const [Name, setName] = useState('');
	const [Description, setDescription] = useState('');
	const [country, setCountry] = useState('');
	const [WebSiteUrl, setWebSiteUrl] = useState('');
	const [FacebookUrl, setFacebookUrl] = useState('');
	const [Edition, setEdition] = useState('');
	const [errorEdition, setErrorEdition] = useState('');
	const [helperTextEdition, setHelperTextEdition] = useState('');

	const [errorName, setErrorName] = useState(false);
	const [errorDescription, setErrorDescription] = useState(false);
	const [errorCountry, setErrorCountry] = useState(false);

	const [helperTextName, setHelperTextName] = useState('');
	const [helperTextDescription, setHelperTextDescription] = useState('');
	const [helperTextCountry, setHelperTextCountry] = useState('');
	const [helperTextWebSiteUrl, setHelperTextWebSiteUrl] = useState('');
	const [helperTextBookId, setHelperTextBookId] = useState('');
	const [helperTextFacebookUrl, setHelperTextFacebookUrl] = useState('');

	const [CountryId, setCountryId] = useState('');
	const [helperTextCountryId, setHelperTextCountryId] = useState('');

	const [Title, setTitle] = useState('');
	const [countryName, setCountryName] = useState('');

	const columns = [
		{
			field: 'title',
			headerName: 'Titre du livre',
			flex: 1
		},
		{
			field: 'edition',
			headerName: 'Edition',
			flex: 1
		},
		{
			field: 'actions',
			headerName: 'Actions',
			flex: 1,
			renderCell: params => {
				const onDeleteClick = e => {
					e.stopPropagation();
					swal({
						text: 'Voulez vous supprimer cet éditeur ?',
						icon: 'warning',
						buttons: ['Non', 'Oui'],
						dangerMode: true
					});
				};

				return <PrizeRow />;
			}
		}
	];

	const classes = useStyles();

	const loadCountries = () => {
		dispatch(getCountries());
	};

	const loadBooks = () => {
		dispatch(getBooks());
	};
	const loadPrizeBooks = () => {
		dispatch(getAllPrizedBooks());
	};
	const loadPrizes = () => {
		dispatch(getPrizes());
	};
	useEffect(() => {
		loadCountries();
		loadBooks();
		loadPrizeBooks();
		loadPrizes();
	}, []);

	const validationTitleError = title => {
		if (title === '') {
			setErrorName(true);
			setHelperTextName('vous devez entrer un titre');
		}
	};
	const onChangeName = e => {
		setName(e.target.value);
		setErrorName(false);
		setHelperTextName('');
		validationTitleError(e.target.value.trim());
	};
	const onChangeDescription = e => {
		setDescription(e.target.value);
		setErrorDescription(false);
		setHelperTextDescription('');
	};
	const validateCountryName = name => {
		if (name === '') {
			setErrorCountry(true);
			setHelperTextCountry('vous devez choisir un pays');
		}
	};

	const onChangeCountryName = (e, value) => {
		setCountryName(value.name);
		setErrorCountry(false);
	};

	const getCountryId = name => {
		for (let i = 0; i < countries.length; i++) {
			if (countries[i].name === name) {
				return countries[i].id;
			}
		}
	};
	const handleChangeTab = (e, newValue) => {
		setValue(newValue);
	};

	const onChangeEdition = e => {
		setEdition(e.target.value);
	};
	const onChangeWebSiteUrl = e => {
		setWebSiteUrl(e.target.value);
	};

	const onChangeFacebookUrl = e => {
		setFacebookUrl(e.target.value);
	};

	const filter = () => {
		if (Title.length > 0) {
			dispatch(getBookByTitle({ Title }));
		}
	};
	const findBookTitleById = id => {
		return (
			books.length > 0 && books.filter(book => book.id === id)[0].title
		);
	};

	const reset = () => {
		setName('');
		setDescription('');
		setWebSiteUrl('');
		setFacebookUrl('');
		setCountryName('');
	};

	const addPrize = () => {
		const newPrize = {
			Name,
			Description,
			CountryId: getCountryId(countryName),
			WebSiteUrl,
			FacebookUrl
		};

		let regexDigitalPass = /(?=.*\d)/;
		validationTitleError(Name);
		validateCountryName(countryName);
		if (Name !== "" && getCountryId(countryName) !== undefined) {
			dispatch(addNewPrize(newPrize))
				.then(unwrapResult)
				.then(originalPromiseResult => {
					swal({
						title: 'Prix ajouté!',
						icon: 'success',
						time: 5000
					});
					//setPrizeId(originalPromiseResult.id);
					setPrizeName(originalPromiseResult.name);
					loadPrizes();
					reset();
				});
		}

	};
	const addPrizedBook = () => {
		const obj = prizes.filter(prize => prize.name === PrizeName)[0];

		if (obj !== undefined) {
			setPrizeId(obj.id);
			const Prize = {
				PrizeId: obj.id,
				BookId,
				Edition
			};

			if (BookId !== null && Edition !== '') {
				dispatch(addPrizeBook(Prize))
					.then(unwrapResult)
					.then(originalPromiseResult => {
						swal({
							title: 'Livre prisé ajouté!',
							icon: 'success',
							time: 5000
						});
						setEdition('');
						loadPrizeBooks();
					});
			}
		}
	};

	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center">
					<div className="pt-10 pb-10">
						<div className="flex flex-col max-w-full min-w-0">
							<Link
								to="/bookPrize/list"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b>Prix</b>
							</Link>
						</div>
						<div className="flex items-center max-w-full">
							<EmojyEvents
								style={{
									fontSize: '40px'
								}}
							/>
							<div className="flex flex-col mx-8">
								<h2>
									{PrizeName !== ''
										? PrizeName
										: 'Nouveau Prix'}
								</h2>
								<p>Détails du prix</p>
							</div>
						</div>
					</div>
				</div>
			}
			contentToolbar={
				<div className={classes.root}>
					<Tabs
						value={value}
						onChange={handleChangeTab}
						indicatorColor="primary"
						textColor="primary"
						variant="scrollable"
						scrollButtons="auto"
						aria-label="scrollable auto tabs example"
						style={{ height: "65px", paddingTop: "10px" }}
					>
						<Tab label="Infos Basiques" {...a11yProps(0)} />
						<Tab label="Livres" {...a11yProps(1)} />
					</Tabs>
				</div>
			}
			content={
				<>
					<TabPanel value={value} index={0}>
						<Box component="form">
							<TextField
								id="outlined-basic"
								onChange={onChangeName}
								label="Titre du prix"
								variant="outlined"
								className="w-full"
								name="name"
								value={Name}
								required
								error={errorName}
								helperText={helperTextName}
							/>
							<TextField
								id="outlined-basic"
								variant="outlined"
								placeholder="Description du livre"
								className="w-full mt-10 h-300px"
								multiline
								rows={Infinity}
								rowsMax={10}
								label="Description"
								onChange={onChangeDescription}
								helperText={helperTextDescription}
								name="description"
								value={Description}
							/>

							<Autocomplete
								id="combo-box-demo"
								options={countries}
								getOptionLabel={option => option.name}
								noOptionsText="pas de pays"
								className="w-full mt-10"
								name="country"
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
										onChange={handleChange}
										required
										error={errorCountry}
										helperText={helperTextCountry}
									/>
								)}
							/>
							<TextField
								id="outlined-basic"
								variant="outlined"
								placeholder="Lien du livre"
								className="w-full mt-10 h-300px"
								label="Lien du site web"
								onChange={onChangeWebSiteUrl}
								helperText={helperTextWebSiteUrl}
								name="WebSiteUrl"
								value={WebSiteUrl}
							/>

							<TextField
								id="outlined-basic"
								variant="outlined"
								placeholder="Lien du page facebook"
								className="w-full mt-10 h-300px"
								label="Lien du page facebook"
								onChange={onChangeFacebookUrl}
								helperText={helperTextFacebookUrl}
								name="FacebookUrl"
								value={FacebookUrl}
							/>
						</Box>
						<Button

							onClick={addPrize}
							variant="contained"
							size="small"
							style={{
								backgroundColor: "#E39320",
								color: "#fff",
								padding: "10px",
								marginTop: "8px",
								width: "130px",
								borderRadius: "5px"
							}}
						>
							Sauvegarder
						</Button>
					</TabPanel>
					<TabPanel value={value} index={1}>
						<Box component="form">
							<Autocomplete
								id="combo-box-demo"
								options={Title.length > 0 ? booksByTitle : []}
								getOptionLabel={option => option.title}
								noOptionsText="pas de livres"
								className="w-full mt-10"
								value={values}
								name="book"
								disableClearable
								disablePortal={true}
								onChange={(e, value) => {
									setBookId(value.id);
									setValues(value);
								}}
								renderInput={params => (
									<TextField
										{...params}
										label="Livres"
										variant="outlined"
										fullWidth
										onChange={e => {
											setTitle(e.target.value);
											if (e.target.value.length > 0) {
												setDisabled(false);
											} else {
												setValues([]);
												setDisabled(true);
											}
										}}
										required
										onKeyUp={filter}
										helperText={helperTextCountryId}
									/>
								)}
							/>
							<TextField
								id="outlined-basic"
								label="Editions"
								placeholder="Editions"
								variant="outlined"
								className="w-full mt-10"
								onChange={onChangeEdition}
								required
								value={Edition}
								error={errorEdition}
								helperText={helperTextEdition}
								disabled={disabled}
							/>
							<Button
								className="save-btn-prize"
								onClick={addPrizedBook}
								variant="contained"
								color="primary"
								style={{
									backgroundColor: "#E39320",
									color: "#fff",
									padding: "10px",
									marginTop: "8px",
									width: "130px",
									borderRadius: "5px"
								}}
							>
								Sauvegarder
							</Button>
						</Box>
						<Box
							style={{
								width: '55%',
								marginTop: '20px',
								height: '200px'
							}}
						>
							<DataGrid
								className={classes.root}
								rows={
									PrizeId !== ''
										? prizedBooks
											.filter(
												prizedBook =>
													prizedBook.prizeId ===
													PrizeId
											)
											.map(prizedBook => ({
												id: prizedBook.id,
												title: findBookTitleById(
													prizedBook.bookId
												),
												edition: prizedBook.edition
											}))
										: []
								}
								rowHeight={50}
								components={{ NoRowsOverlay }}
								columns={columns}
								pageSize={2}
								disableColumnMenu
							/>
						</Box>
					</TabPanel>
				</>
			}
		/>
	);
};

export default NewPrize;
