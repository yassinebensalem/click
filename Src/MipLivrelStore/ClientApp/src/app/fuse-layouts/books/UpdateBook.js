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
	Checkbox,
	CircularProgress
} from '@material-ui/core';
import ArrowBack from '@material-ui/icons/ArrowBack';
import DownloadIcon from '@material-ui/icons/CloudDownload';
import React, { useEffect, useState, useRef } from 'react';
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
import { Link, useHistory, useParams } from 'react-router-dom';
import { getCountries } from 'app/store/countrySlice';
import { getLanguages } from 'app/store/languageSlice';
import { getCategories } from 'app/store/categorySlice';
import { getAuthors } from 'app/store/authorSlice';
import { getActiveEditorsByRole, getEditors } from 'app/store/editorSlice';
import { createBook, getBooks, updateBook } from 'app/store/bookSlice';
import { unwrapResult } from '@reduxjs/toolkit';
import swal from 'sweetalert';
import { getAuthorsWithoutAccount } from '../../store/authorWithoutAccountSlice';
import { decode } from 'jsonwebtoken';

const UpdateBook = () => {
	const [disabled, setDisabled] = useState(true);
	const [selectedDate, setSelectedDate] = useState(new Date());

	const [successMsg, setSuccessMsg] = useState('');
	const [open, setOpen] = useState(false);
	const dispatch = useDispatch();

	const useStyles = makeStyles(theme => ({
		root: {
			flexGrow: 1,
			width: '100%',
			backgroundColor: theme.palette.background.paper
		}
	}));

	const [value, setValue] = useState(0);
	const [books, setBooks] = useState([]);
	const [categoryName, setCategoryName] = useState('');
	const [languageName, setLanguageName] = useState('');
	const [countryName, setCountryName] = useState('');
	const [editorName, setEditorName] = useState('');
	const [Title, setTitle] = useState('');
	const [coverPath, setCoverPath] = useState('');
	const [Price, setPrice] = useState('');
	const [Description, setDescription] = useState('');
	const [PageNumbers, setPageNumbers] = useState('');
	const [PublicationDate, setPublicationDate] = useState('');
	const [ISBN, setISBN] = useState('');
	const [ISSN, setISSN] = useState('');
	const [EISBN, setEISBN] = useState('');
	const [errorTitle, setErrorTitle] = useState(false);
	const [errorDescription, setErrorDescription] = useState(false);
	const [errorPageNumbers, setErrorPageNumbers] = useState(false);
	const [errorISBN, setErrorISBN] = useState(false);
	const [errorISSN, setErrorISSN] = useState(false);
	const [errorEISBN, setErrorEISBN] = useState(false);
	const [errorCategories, setErrorCategories] = useState(false);
	const [errorCountry, setErrorCountry] = useState(false);
	const [errorLanguage, setErrorLanguage] = useState(false);
	const [errorPrice, setErrorPrice] = useState(false);
	const [errorAuthor, setErrorAuthor] = useState(false);
	const [errorEditor, setErrorEditor] = useState(false);
	const [helperTextISBN, setHelperTextISBN] = useState('');
	const [helperTextTitle, setHelperTextTitle] = useState('');
	const [helperTextCategoryName, setHelperTextCategoryName] = useState('');
	const [helperTextDescription, setHelperTextDescription] = useState('');
	const [helperTextAuthorName, setHelperTextAuthorName] = useState('');
	const [helperTextEditorName, setHelperTextEditorName] = useState('');
	const [helperTextPagesNumber, setHelperTextPagesNumber] = useState('');
	const [helperTextISSN, setHelperTextISSN] = useState('');
	const [helperTextEISBN, setHelperTextEISBN] = useState('');
	const [helperTextCountryName, setHelperTextCountryName] = useState('');
	const [helperTextLanguageName, setHelperTextLanguageName] = useState('');
	const [authorId, setAuthorId] = useState('');
	const [helperTextPrice, setHelperTextPrice] = useState('');
	const [categoryId, setCategoryId] = useState('');
	const [editorId, setEditorId] = useState('');
	const [countryId, setCountryId] = useState('');
	const [languageId, setLanguageId] = useState('');
	const [isFree, setIsFree] = useState(false);
	const [bookStatus, setBookStatus] = useState('');
	const [bookStatusId, setBookStatusId] = useState('');
	const { countries } = useSelector(state => state.country);
	const { languages } = useSelector(state => state.language);
	const { categories } = useSelector(state => state.category);
	const { editors } = useSelector(state => state.editor);
	const { authorsWithoutAccount } = useSelector(state => state.authorWithoutAccount);
	const {loadingUpdateBook} = useSelector(state => state.book);
	const [CoverFile, setCoverFile] = useState('');
	const [errorCoverFile, setErrorCoverFile] = useState('');
	const [helperTextCoverFile, setHelperTextCoverFile] = useState('');
	const [CoverName, setCoverName] = useState('');
	const [PDFFile, setPDFFile] = useState('');
	const [PDFName, setPDFName] = useState('');
	const [errorPDFFile, setErrorPDFFile] = useState('');
	const [helperTextPDFFile, setHelperTextPDFFile] = useState('');
	const [bookId, setBookId] = useState('');
	const [enabled, setEnabled] = useState(false);
	const { id } = useParams();
	const classes = useStyles();
	let regexPrice = /(?=.*\d)/;
	const [currentAuthorId, setCurrentAuthorId] = useState('');
	const [currentEditorId, setCurrentEditorId] = useState('');
	const [currentCountryId, setCurrentCountryId] = useState('');
	const [currentLanguageId, setCurrentLanguageId] = useState('');
	const [currentCategoryId, setCurrentCategoryId] = useState('');
	const [currentStateId, setCurrentStateId] = useState('');
	const [changeCountryId, setChangeCountryId] = useState(false);
	const [changeLanguageId, setChangeLanguageId] = useState(false);
	const [changeCategoryId, setChangeCategoryId] = useState(false);
	const [changeStateId, setChangeSgtateId] = useState(false);
	const [changeEditorId, setChangeEditorId] = useState(false);
	const photoRef = useRef(null);
	const [coverUpdated, setCoverUpdated] = useState(null);
	const [changeAuthorId, setChangeAuthorId] = useState(false);
	const {token} = useSelector(state => state.auth);
	const {history} = useHistory();
	const [urlPdf, setUrlPdf] = useState('');
	const [urlCover, setUrlCover] = useState('');

	const status = [
		{
			id: 1,
			name: 'Crée'
		},
		{
			id: 2,
			name: 'Rejeté'
		},
		{
			id: 3,
			name: 'Publié'
		},
		{
			id: 4,
			name: 'Non Publié'
		}
	];
	const categoriesParsed = categories.map(row => ({
		...row,
		categoryName: JSON.parse(row['categoryName'])
	}));

	const authorsMapped = authorsWithoutAccount.map(author => ({
		...author,
		authorName: `${author.firstName} ${author.lastName}`
	}));
	const hasOneDecimalPoint = number => {

		if(number.split('').filter(c => c === '.').length == 1 ||
		number.split('').filter(c => c === '.').length == 0 ) 
		
		{
		
           return true;
		}else {
			return false;
		}		
	}

	

	const hasSlash = number => {
		if(number.split('').filter(c => c === '/').length > 0) 
		{
		
           return true;
		}else {
			return false;
		}
	}
	const onChangeCategoryName = (e, value) => {

		setCategoryId(value.id);
		setErrorCategories(false);
		setHelperTextCategoryName('');
		setChangeCategoryId(true);
	};

	const onChangeLanguageName = (e, value) => {
		setLanguageId(value.id);
		setErrorLanguage(false);
		setHelperTextLanguageName('');
		setChangeLanguageId(true);
	};

	const onChangeCountryName = (e, value) => {
		setCountryId(value.id);
		setErrorCountry(false);
		setHelperTextCountryName('');
		setChangeCountryId(true);
	};


	const onChangeAuthorId = (e, value) => {
		setAuthorId(value.id);
		setErrorAuthor(false);
		setHelperTextAuthorName('');
		setChangeAuthorId(true);
		console.log(value)
	};

	const onChangeEditorEmail = (e, value) => {
		setEditorId(value.id);
		setErrorEditor(false);
		setHelperTextEditorName('');
		setChangeEditorId(true)
	};

	const onChangeEditorName = (e, value) => {
		setEditorName(value.userName);
		setErrorEditor(false);
		setHelperTextEditorName('');
	};
	const validationTitleError = title => {
		if (title === "") {
			setHelperTextTitle('vous devez entrer un titre du livre');
			setErrorTitle(true);
			setEnabled(false);
		}
		else {
			setHelperTextTitle('');
			setEnabled(true);
		}
	}
	const onChangeTitle = e => {
		setTitle(e.target.value);
		setErrorTitle(false);
		setHelperTextTitle('');
		validationTitleError(e.target.value.trim());
	};

	const validationDescriptionError = description => {
		if (description === "") {
			setHelperTextDescription('vous devez entrez une description');
			setErrorDescription(true);
			setEnabled(false);
		}
		else if (description.trim().length < 2) {
			setHelperTextDescription(
				'la description doit avoir au moins 2 caractéres'
			);
			setEnabled(false);
			setErrorDescription(true);
		} else {
			setHelperTextDescription('');
			setEnabled(true);
		}

	}
	const onChangeDescription = e => {
		setDescription(e.target.value);
		setErrorDescription(false);
		setHelperTextDescription('');
		validationDescriptionError(e.target.value.trim());
	};
	const validationPriceError = price => {
		if (price === '') {
			setErrorPrice(true);
			setHelperTextPrice('vous devez entrer un prix');
			setEnabled(false)
		}
		else if(hasOneDecimalPoint(price) === false || hasSlash(price) === true) {
			setErrorPrice(true);
			setHelperTextPrice('vous devez entrer un format de prix correct');
			setEnabled(false);
		}
		else {
			setEnabled(true);
		}
	}
	const onChangePrice = e => {
		setPrice(e.target.value);
		setErrorPrice(false);
		setHelperTextPrice('');
		validationPriceError(e.target.value.trim());
	};

	const validatePageNumberError = pageNumbers => {
		if (pageNumbers === '') {
			setErrorPageNumbers(true);
			setHelperTextPagesNumber('vous devez entrer un nombre de pages');
			setEnabled(false);
		} else if (PageNumbers < 0) {
			setErrorPageNumbers(true);
			setHelperTextPagesNumber(
				'vous ne devez pas entrer un nombre négatif'
			);
			setEnabled(false);
		} else {
			setHelperTextPagesNumber('');
			setEnabled(true);
		}
	}
	const onChangePageNumbers = e => {
		setPageNumbers(e.target.value);
		setErrorPageNumbers(false);
		setHelperTextPagesNumber('');
		validatePageNumberError(e.target.value.trim());
	};
	const validateISBNError = isbn => {
		if (isbn === '') {
			setErrorISBN(true);
			setHelperTextISBN("vous devez entrer un isbn");
			setEnabled(false)
		} else {
			setEnabled(true);
		}
	}
	const onChangeISBN = e => {
		setISBN(e.target.value);
		setErrorISBN(false);
		setHelperTextISBN('');
		validateISBNError(e.target.value.trim());
	};


	const onChangeCover = e => {
		if(e.target.files[0].size/1024/1024 > 40) {
			setErrorCoverFile(true);
			setHelperTextCoverFile('la taille du fichier est très grande');
		}else {
			setCoverFile(e.target.files[0]);
			setCoverName(e.target.files[0].name);
			setUrlCover(URL.createObjectURL(e.target.files[0]));
			setUrlPdf(URL.createObjectURL(e.target.files[0]));
			setErrorCoverFile(false);
			setHelperTextCoverFile('');
			setEnabled(true);
		}
		
	};

	const onChangePdf = e => {
		setErrorPDFFile(false);
		setPDFFile(e.target.files[0]);
		setPDFName(e.target.files[0].name);
		setHelperTextPDFFile('');
		setEnabled(true);
	};

	

	const onChangeStatusId = (e, value) => {
		setBookStatus(value.id);
		setEnabled(true);
	};

	const onPress = evt => {
		if (
			(evt.which != 8 && evt.which != 0 && evt.which < 48) ||
			evt.which > 57
		) {
			evt.preventDefault();
		}
	};
	const onPressPrice = evt => {
		if (
			(evt.which != 8 && evt.which != 0 && ((evt.which < 45) || evt.which > 57)) ||
			evt.target.value.length > 7 
	
		) {
			evt.preventDefault();
		}
	};

	
	useEffect(() => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		loadAuthors();
		loadActiveEditors();
		loadCategories();
		loadCountries();
		loadLanguages();
		dispatch(getBooks())
			.then(unwrapResult)
			.then(res => {
				const obj = res.filter(obj => obj.id === id)[0];
				if (obj) {
					setBookId(obj.id);
					setTitle(obj.title);
					setCurrentCategoryId(obj.categoryId);
					setCurrentAuthorId(obj.authorId);
					setCurrentEditorId(obj.publisherId);
					setISBN(obj.isbn);
					setISSN(obj.issn);
					setEISBN(obj.eisbn);
					setCurrentLanguageId(obj.languageId);
					setCurrentCountryId(obj.countryId);
					setSelectedDate(obj.publicationDate);
					setPrice(obj.price);
					setPageNumbers(obj.pageNumbers);
					setDescription(obj.description);
					setBookStatus(obj.status);
					setCoverPath(obj.coverPath);
				}
			});
		if (changeCountryId === true) {
			setEnabled(true);
		}
		if (changeCategoryId === true) {
			setEnabled(true)
		}
		if (changeLanguageId === true) {
			setEnabled(true)
		}
		if (changeEditorId === true) {
			setEnabled(true)
		}
		if (changeAuthorId === true) {
			setEnabled(true)
		}
	}, [id, changeCountryId, changeCategoryId, changeLanguageId, changeAuthorId, changeEditorId]);
	const findCategoryName = () => {
		let obj = null;
		if (changeCategoryId === true) {
			obj = categoriesParsed.filter(
				category => category.id === categoryId
			)[0];
		} else {
			obj = categoriesParsed.filter(
				category => category.id === currentCategoryId
			)[0];
		}


		return obj !== undefined && obj.categoryName.fr;
	};

	const findAuthorName = () => {
		let obj;
		if (changeAuthorId === true) {
			obj = authorsMapped.filter(author => author.id === authorId)[0];
		} else {
			obj = authorsMapped.filter(author => author.id === currentAuthorId)[0];
		}


		return obj !== undefined && obj.authorName;
	};


	const findEditorRaisonSocial = () => {
		let obj;
		if (changeEditorId === true) {
			obj = editors.filter(editor => editor.id === editorId)[0];
		} else {
			obj = editors.filter(editor => editor.id === currentEditorId)[0];
		}

		return obj !== undefined && obj.raisonSocial;
	};
	const findEditorLastName = () => {
		let obj;
		if (changeEditorId === true) {
			obj = editors.filter(editor => editor.id === editorId)[0];
		} else {
			obj = editors.filter(editor => editor.id === currentEditorId)[0];
		}

		return obj !== undefined && `${obj.lastName}`;
	};

	const findCountryName = () => {
		let obj;
		if (changeCountryId === true) {
			obj = countries.filter(country => country.id === countryId)[0];
		} else {
			obj = countries.filter(country => country.id === currentCountryId)[0];
		}

		return obj !== undefined && obj.name;
	};

	const findLanguageName = () => {
		let obj;
		if (changeLanguageId === true) {
			obj = languages.filter(language => language.id === languageId)[0];
		} else {
			obj = languages.filter(language => language.id === currentLanguageId)[0];
		}

		return obj !== undefined && obj.name;
	};

	const loadActiveEditors = () => {
		dispatch(getActiveEditorsByRole());
	};
	const loadAuthors = () => {
		dispatch(getAuthorsWithoutAccount());
	};
	const loadCountries = () => {
		dispatch(getCountries());
	};
	const loadLanguages = () => {
		dispatch(getLanguages());
	};
	const loadCategories = () => {
		dispatch(getCategories());
	};

	const getCategoryId = name => {
		for (let i = 0; i < categoriesParsed.length; i++) {
			if (categoriesParsed[i].categoryName.fr === name) {
				return categoriesParsed[i].id;
			}
		}
	};


	const getAuthorId = name => {
		for (let i = 0; i < authorsMapped.length; i++) {
			if (authorsMapped[i].authorName === name) {
				return authorsMapped[i].id;
			}
		}
	};


	const getEditorId = (raisonSocial) => {
		for (let i = 0; i < editors.length; i++) {
			if (editors[i].raisonSocial === raisonSocial) {
				return editors[i].id;
			}
		}
	};

	const getLanguageId = name => {
		for (let i = 0; i < languages.length; i++) {
			if (languages[i].name === name) {
				return languages[i].id;
			}
		}
	};

	const getCountryId = name => {
		for (let i = 0; i < countries.length; i++) {
			if (countries[i].name === name) {
				return countries[i].id;
			}
		}
	};

	const onChangeDate = (e, row) => {
		setSelectedDate(row);
	};

	const findBookStatusName = id => {
		for (let i = 0; i < status.length; i++) {
			if (status[i].id === id) {
				return status[i].name;
			}
		}
	};

	console.log(findAuthorName());

	const editBook = () => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		const formData = new FormData();

		formData.append('Id', bookId);
		formData.append('Title', Title);
		formData.append('Price', Price.toString().replace('.',','));
		formData.append('Description', Description);
		formData.append('PageNumbers', PageNumbers);
		formData.append('PublicationDate', selectedDate);
		formData.append('ISBN', ISBN);
		formData.append('AuthorId', getAuthorId(findAuthorName()));
		formData.append('PublisherId', getEditorId(findEditorRaisonSocial()));
		formData.append('CategoryId', getCategoryId(findCategoryName()));
		formData.append('LanguageId', getLanguageId(findLanguageName()));
		formData.append('CountryId', getCountryId(findCountryName()));
		formData.append('IsFree', isFree);
		formData.append('Status', bookStatus);
		if (photoRef.current.value !== null) {
			formData.append('CoverFile', CoverFile);
		}


		formData.append('PDFFile', PDFFile);
		dispatch(updateBook(formData))
			.then(unwrapResult)
			.then(originalPromiseResult => {
				swal({
					title: 'Livre modifié!',
					icon: 'success'
				});
			})
			.catch(rejectedValueOrSerializedError => {
				// handle result here
			});
	};


	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center">
					<div className="pt-10 pb-10">
						<div className="flex flex-col max-w-full min-w-0">
							<Link
								to="/book/list"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b>Livres</b>
							</Link>
						</div>
						<div className="flex items-center max-w-full">
							<img
								src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${coverPath}`}
								alt="user"
								className="w-32 sm:w-48 rounded"
								width={40}
								height={40}
							/>
							<div className="flex flex-col mx-8">
								<h2>{Title}</h2>
								<p>Détails du livre</p>
							</div>
						</div>
					</div>
					{
						loadingUpdateBook ?
						<CircularProgress color='#F75454' size={20} />
						:
						<Button
						onClick={editBook}
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
					<Box component="form">
						<TextField
							id="outlined-basic"
							onChange={onChangeTitle}
							label="Nom du livre"
							variant="outlined"
							className="w-full"
							name="name"
							value={Title}
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
							name="description"
							value={Description}
						/>
						<Autocomplete
							id="combo-box-demo"
							onChange={onChangeCategoryName}
							options={categoriesParsed}
							getOptionLabel={option => option.categoryName.fr}
							className="w-full mt-10"
							name="category"
							value={{
								categoryName: {
									fr: findCategoryName()
								}
							}}
							disableClearable
							disablePortal={true}
							renderInput={params => (
								<TextField
									{...params}
									label="Categories"
									variant="outlined"
									fullWidth
									required
								/>
							)}
						/>
						<Autocomplete
							id="combo-box-demo"
							options={authorsMapped}
							onChange={onChangeAuthorId}
							getOptionLabel={option => option.authorName}
							className="w-full mt-10"
							name="author"
							disableClearable
							disablePortal={true}
							value={{ authorName: findAuthorName() }}

							renderInput={params => (
								<TextField
									{...params}
									label="Nom de l'auteur"
									variant="outlined"
									fullWidth
									error={errorAuthor}
									helperText={helperTextAuthorName}
									required

								/>
							)}
						/>
						<Autocomplete
							id="combo-box-demo"
							onChange={onChangeEditorEmail}
							options={editors}
							getOptionLabel={option => option.raisonSocial}
							value={{ raisonSocial: findEditorRaisonSocial() }}
							className="w-full mt-10"
							name="editor"
							disableClearable
							disablePortal={true}
							renderInput={params => (
								<TextField
									{...params}
									label="Nom de l'editeur"
									variant="outlined"
									fullWidth
									
								/>
							)}
						/>
						<TextField
							id="outlined-basic"
							label="Nombre de pages"
							variant="outlined"
							className="w-full mt-10"
							name="pages"
							onChange={onChangePageNumbers}
							value={PageNumbers}
							onKeyPress={onPress}
		
							error={errorPageNumbers}
							helperText={helperTextPagesNumber}
							required
						/>
						<TextField
							id="outlined-basic"
							label="ISBN"
							variant="outlined"
							className="w-full mt-10"
							onChange={onChangeISBN}
							value={ISBN}
						/>
						<Autocomplete
							id="combo-box-demo"
							options={countries}
							getOptionLabel={option => option.name}
							className="w-full mt-10"
							name="country"
							onChange={onChangeCountryName}
							value={{ name: findCountryName() }}
							disableClearable
							disablePortal={true}
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
						<Autocomplete
							id="combo-box-demo"
							options={languages}
							onChange={onChangeLanguageName}
							getOptionLabel={option => option.name}
							className="w-full mt-10"
							name="language"
							disableClearable
							disablePortal={true}
							value={{ name: findLanguageName() }}
							renderInput={params => (
								<TextField
									{...params}
									label="Languages"
									variant="outlined"
									fullWidth
									required
								/>
							)}
						/>
						<KeyboardDatePicker
							autoOk
							variant="inline"
							inputVariant="outlined"
							label="Date de création"
							value={selectedDate}
							format={'YYYY-MM-DD'}
							InputAdornmentProps={{ position: 'start' }}
							onChange={onChangeDate}
							className="w-full mt-10"
						/>
						<TextField
							id="outlined-basic"
							label="Prix"
							variant="outlined"
							className="w-full mt-10"
							name="price"
							onChange={onChangePrice}
							value={Price}
							error={errorPrice}
							helperText={helperTextPrice}
							onKeyPress={onPressPrice}
							required
							
						/>
						<p className="mt-10 text-gray">
							<label>PDF du livre</label>
							<div className='flex items-center'>
							<TextField
								id="outlined-basic"
								variant="outlined"
								className="w-full mt-10"
								type="file"
								onChange={onChangePdf}
								name="filePath"
							/>
							<a href={urlPdf} 
								style={{background:"#f50", color:"#fff", textDecoration:"none",
							  width:"40px", height:"40px", borderRadius:"50%", display:"flex", justifyContent:"center",
							  alignItems:"center"
							} }
								download
								><DownloadIcon/></a>
						</div>
						</p>
						<p className="mt-10 text-gray">
							<label>Livre page de garde</label>
							<div className='flex items-center'>
							<TextField
								id="outlined-basic"
								variant="outlined"
								className="w-full mt-10"
								type="file"
								name="coverPath"
								onChange={onChangeCover}
								inputRef={photoRef}
							/>
								<a href={urlCover}
							style={{background:"#f50", color:"#fff", textDecoration:"none",
							width:"40px", height:"40px", borderRadius:"50%", display:"flex", justifyContent:"center",
							  alignItems:"center"
							} }
							 download
								>
								<DownloadIcon/>	
								</a>
						</div>		
						</p>
						<Autocomplete
							id="combo-box-demo"
							onChange={onChangeStatusId}
							options={status}
							getOptionLabel={option => option.name}
							value={{ name: findBookStatusName(bookStatus) }}
							className="w-full mt-10"
							name="bookStatus"
							disableClearable
							renderInput={params => (
								<TextField
									{...params}
									label="Etat du livre"
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

export default UpdateBook;
