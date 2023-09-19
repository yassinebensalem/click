import React, { useState, Fragment, useEffect, useRef } from 'react';
import {
	TextField,
	Box,
	AppBar,
	makeStyles,
	Tabs,
	Tab,
	Button
} from '@material-ui/core';
import TabPanel from '../shared-components/TabPanel';
import FusePageCarded from '@fuse/core/FusePageCarded';
import a11yProps from '../a11props';
import { KeyboardDatePicker } from '@material-ui/pickers';
import AutoComplete from '@material-ui/lab/Autocomplete/Autocomplete';
import { DataGrid } from '@material-ui/data-grid';
import { useSelector, useDispatch } from 'react-redux';
import { getBooks } from 'app/store/bookSlice';
import { Add, ArrowBack } from '@material-ui/icons';
import { Link, useHistory } from 'react-router-dom';
import { getCountries } from 'app/store/countrySlice';
import {
	addNewPromotion,
	addPromotionBook,
	getAllPromotions,
	getAllPromotionBooks,
	resetArray
} from 'app/store/promotionSlice';
import moment from 'moment';
import { unwrapResult } from '@reduxjs/toolkit';
import swal from 'sweetalert';
import { decode } from 'jsonwebtoken';

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
const NewPromotion = () => {
	const [value, setValue] = useState(0);

	const handleChangeTab = (e, newValue) => {
		setValue(newValue);
	};
	const classes = useStyles();
	const { books } = useSelector(state => state.book);
	const {history} = useHistory();

	const dispatch = useDispatch();
	const promotionsType = [
		{
			id: 0,
			label: 'Gratuit'
		},
		{
			id: 1,
			label: 'Remise'
		}
	];

	const [promotionType, setPromotionType] = useState('');
	const [promotionTypeError, setPromotionTypeError] = useState(false);
	const [promotionTypeHelperText, setPromotionTypeHelperText] = useState('');
	const [bookId, setBookId] = useState('');
	const [bookNameError, setBookNameError] = useState(false);
	const [bookNameHelperText, setBookNameHelperText] = useState('');
	const [PromotionName, setPromotionName] = useState('');
	const [Percentage, setPercentage] = useState('');
	const [startDate, setStartDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	const [endDate, setEndDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	const [description, setDescription] = useState('');
	const [countryName, setCountryName] = useState('');
	const [countryNameError, setCountryNameError] = useState(false);
	const [countryNameHelperText, setCountryNameHelperText] = useState('');
	const { countries } = useSelector(state => state.country);
	const { promotions, promotionBooks } = useSelector(
		state => state.promotion
	);
	const [photoPath, setPhotoPath] = useState('');
	const [promotionId, setPromotionId] = useState('');

	const [submitted, setSubmitted] = useState(false);
	const {token} = useSelector(state => state.auth);


	const getLabel = id => {
		if (id === 0) {
			return 'Gratuit';
		} else if (id === 1) {
			return 'Remise';
		} else {
			return undefined;
		}
	};
	const validatePromotionType = type => {
		if (type === '') {
			setPromotionTypeError(true);
			setPromotionTypeHelperText('veuillez choisir un type');
		}
	};
	const validateCountry = id => {
		if (id === undefined) {
			setCountryNameError(true);
			setCountryNameHelperText('veuillez choisir un pays');
		}
	};
	const validateBookName = id => {
		if (id === '') {
			setBookNameError(true);
			setBookNameHelperText('veuillez choisir un livre');
		}
	};
	const columns = [
		{
			field: '',
			headerName: '',
			flex: 1,
			renderCell: params => {
				const bookObjCoverPath = books.filter(
					book => book.id === params.row.bookId
				)[0].coverPath;

				return (
					<div
						style={{
							backgroundColor: '#eee'
						}}
						className="rounded p-2"
					>
						<img
							src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${bookObjCoverPath}`}
							alt="avatar"
							style={{
								width: '52px',
								height: '52px',
								objectFit: 'cover'
							}}
							className="w-full block rounded"
						/>
					</div>
				);
			}
		},
		{
			field: 'name',
			headerName: 'Nom du livre',
			flex: 2
		}
	];
	const photoRef = useRef(null);
	const onPress = evt => {
		if (
			(evt.which != 8 && evt.which != 0 && evt.which < 48) ||
			evt.which > 57
		) {
			evt.preventDefault();
		}
	};

	const onChangePromotionType = (e, value) => {
		setPromotionType(value.id);
		setPromotionTypeError(false);
		setPromotionTypeHelperText('');
	};
	const onChangeBookId = (e, value) => {
		setBookId(value.id);
		setBookNameError(false);
		setBookNameHelperText('');
	};

	const onChangePromotionName = e => {
		if (e.target.value === '') {
			reset();
		}
		setPromotionName(e.target.value);
	};

	const onChangeDescription = e => {
		setDescription(e.target.value);
	};

	const onChangeStartDate = (e, row) => {
		setStartDate(row);
	};

	const onChangeEndDate = (e, row) => {
		setEndDate(row);
	};
	const onChangePercentage = e => {
		setPercentage(e.target.value);
	};

	const getCountryId = name => {
		for (let i = 0; i < countries.length; i++) {
			if (countries[i].name === name) {
				return countries[i].id;
			}
		}
	};

	const onChangeCountryName = (e, value) => {
		setCountryName(value.name);
		setCountryNameError(false);
		setCountryNameHelperText('');
	};

	const getBookName = () => {
		if (bookId) {
			return (
				books.length > 0 &&
				books.filter(book => book.id === bookId)[0].title
			);
		} else {
			return undefined;
		}
	};

	const onChangePhotoPath = e => {
		setPhotoPath(e.target.files[0]);
	};
	const reset = () => {
		setDescription('');
		setStartDate(moment(new Date()).format('YYYY-MM-DD'));
		setEndDate(moment(new Date()).format('YYYY-MM-DD'));
		setCountryName('');
		setPromotionType(undefined);
	};

	const addPromotion = () => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		const formData = new FormData();

		formData.append('Name', PromotionName);
		formData.append('PromotionType', promotionType);
		formData.append('StartDate', startDate);
		formData.append('EndDate', endDate);
		formData.append('Description', description);
		formData.append('CountryId', getCountryId(countryName));
		formData.append('Image', photoPath);
		formData.append('Percentage', Percentage);
		validatePromotionType(promotionType);
		validateCountry(getCountryId(countryName));

		dispatch(addNewPromotion(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				// handle result here
				swal({
					title: 'Promotion ajoutée!',
					icon: 'success'
				});
				setSubmitted(true);
				reset();
				dispatch(getAllPromotions());
				setPromotionName(originalPromiseResult.name);
			})
			.catch(rejectedValueOrSerializedError => { });
	};

	const getBookNameById = id => {
		if (id) {
			return (
				books.length > 0 &&
				books.filter(book => book.id === id)[0].title
			);
		} else {
			return undefined;
		}
	};
	useEffect(() => {
		dispatch(getBooks());
		dispatch(getCountries());
		dispatch(getAllPromotionBooks());
		dispatch(getAllPromotions());
	}, []);

	const addBookToPromotion = () => {
		const obj =
			promotions.length > 0 &&
			promotions.filter(promotion => promotion.name === PromotionName)[0];
		setPromotionId(obj.id);
		validateBookName(bookId);

		dispatch(
			addPromotionBook({
				PromotionId: obj.id,
				BookId: bookId
			})
		)
			.then(unwrapResult)
			.then(originalPromiseResult => {
				// handle result here
				swal({
					title: 'Livre ajouté au promotion!',
					icon: 'success'
				});

				dispatch(getAllPromotionBooks());
			})
			.catch(rejectedValueOrSerializedError => { });
	};

	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center">
					<div className="pt-10 pb-10">
						<div className="flex flex-col max-w-full min-w-0">
							<Link
								to="/promotion/list"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b className="hidden sm:flex mx-4 font-medium">
									Promotions
								</b>
							</Link>
						</div>
						<div className="flex items-center max-w-full">
							<span
								class="material-icons MuiIcon-root-309 list-item-icon text-20 flex-shrink-0 MuiIcon-colorAction-312"
								aria-hidden="true"
								style={{ fontSize: '40px' }}
							>
								campaign
							</span>
							<div className="flex flex-col mx-8">
								<h2 className='MuiTypography-root MuiTypography-body1 text-16 sm:text-20 truncate font-semibold muiltr-ehddlr"'>
									{PromotionName !== ''
										? PromotionName
										: 'Nouvelle Promotion'}
								</h2>
								<span className="mt-4 text-xs">
									Détails du promotion
								</span>
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
						style={{ height: '65px', paddingTop: '10px' }}
					>
						<Tab label="Infos Basiques" {...a11yProps(0)} />
						{submitted == true && (
							<Tab label="Livres" {...a11yProps(1)} />
						)}
					</Tabs>
				</div>
			}
			content={
				<Fragment>
					<TabPanel value={value} index={0}>
						<Box component="form">
							<TextField
								id="outlined-basic"
								placeholder="Nom"
								label="Nom"
								variant="outlined"
								className="w-full"
								name="name"
								onChange={onChangePromotionName}
							/>
							<AutoComplete
								id="combo-box-demo"
								onChange={onChangePromotionType}
								options={promotionsType}
								//key={activateChangeAuthor}
								getOptionLabel={option => option.label}
								value={{
									//authorName
									label: getLabel(promotionType)
								}}
								className="w-full mt-10"
								name="type"
								disablePortal={true}
								disableClearable
								required
								renderInput={params => (
									<TextField
										{...params}
										label="Type"
										variant="outlined"
										fullWidth
										required
										error={promotionTypeError}
										helperText={promotionTypeHelperText}
									/>
								)}
							/>
							<KeyboardDatePicker
								autoOk
								variant="inline"
								inputVariant="outlined"
								label="Date de début"
								format="YYYY/MM/DD"
								InputAdornmentProps={{ position: 'start' }}
								className="w-full mt-10"
								onChange={onChangeStartDate}
								value={startDate}
							/>
							<KeyboardDatePicker
								autoOk
								variant="inline"
								inputVariant="outlined"
								label="Date de fin"
								format="YYYY/MM/DD"
								InputAdornmentProps={{ position: 'start' }}
								className="w-full mt-10"
								onChange={onChangeEndDate}
								value={endDate}
							/>
							<AutoComplete
								id="combo-box-demo"
								options={countries}
								getOptionLabel={option => option.name}
								className="w-full mt-10"
								onChange={onChangeCountryName}
								name="country"
								noOptionsText="pas de pays"
								value={{ name: countryName }}
								disableClearable
								renderInput={params => (
									<TextField
										{...params}
										label="Pays"
										variant="outlined"
										fullWidth
										required
										error={countryNameError}
										helperText={countryNameHelperText}
									/>
								)}
							/>
						</Box>
						{promotionType === 1 && (
							<Fragment>
								<TextField
									id="outlined-basic"
									placeholder="Pourcentage"
									label="Pourcentage"
									variant="outlined"
									className="w-full mt-10"
									required
									onKeyPress={onPress}
									onChange={onChangePercentage}
								/>
								<p className="mt-10 text-gray">
									<label>Image(maxWidth: 450px,
									maxHeight: 234px)
									</label>

									<TextField
										id="outlined-basic"
										variant="outlined"
										className="w-full mt-10"
										type="file"
										name="filePath"
										required
										onChange={onChangePhotoPath}
										inputRef={photoRef}
									/>
								</p>

								<TextField
									id="outlined-basic"
									variant="outlined"
									placeholder="Description"
									className="w-full mt-10 h-300px"
									multiline
									rows={Infinity}
									rowsMax={10}
									label="Description"
									name="description"
									required
									onChange={onChangeDescription}
								/>
							</Fragment>
						)}
						<Button
							onClick={addPromotion}
							variant="contained"
							size="small"
							style={{
								backgroundColor: '#E39320',
								color: '#fff',
								padding: '10px',
								marginTop: '8px',
								width: '130px',
								borderRadius: '5px'
							}}
						>
							Sauvegarder
						</Button>
					</TabPanel>
					<TabPanel value={value} index={1}>
						<Box component="form">
							<AutoComplete
								id="combo-box-demo"
								options={books}
								getOptionLabel={option => option.title}
								value={{
									title: getBookName()
								}}
								noOptionsText="pas de livres"
								className="w-full mt-10"
								name="book"
								disableClearable
								disablePortal={true}
								onChange={onChangeBookId}
								renderInput={params => (
									<TextField
										{...params}
										label="Livres"
										variant="outlined"
										fullWidth
										required
										error={bookNameError}
										helperText={bookNameHelperText}
									/>
								)}
							/>
							<Button
								variant="contained"
								size="small"
								style={{
									backgroundColor: '#E39320',
									color: '#fff',
									padding: '10px',
									marginTop: '8px',
									width: '130px',
									borderRadius: '5px'
								}}
								onClick={addBookToPromotion}
							>
								Ajouter
								<Add className="ml-5" />
							</Button>
						</Box>
						<Box
							style={{
								width: '30%',
								marginTop: '20px',
								height: '400px'
							}}
						>
							<DataGrid
								className={classes.root}
								rows={
									promotionBooks !== undefined
										? promotionBooks
											.filter(
												promotionBook =>
													promotionBook.promotionId ===
													promotionId
											)
											.map(promotion => ({
												...promotion,
												name: getBookNameById(
													promotion.bookId
												)
											}))
										: []
								}
								rowHeight={50}
								columns={columns}
								pageSize={2}
								disableColumnMenu
							/>
						</Box>
					</TabPanel>
				</Fragment>
			}
		/>
	);
};

export default NewPromotion;
