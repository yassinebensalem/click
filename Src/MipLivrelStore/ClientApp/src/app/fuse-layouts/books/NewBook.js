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
    Button,
    CircularProgress
} from '@material-ui/core';
import ArrowBack from '@material-ui/icons/ArrowBack';
import React, { useEffect, useRef, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import styled from 'styled-components';
import book from '../../../images/book.png';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';

import TabPanel from '../shared-components/TabPanel';
import a11yProps from '../a11props';
import Autocomplete from '@material-ui/lab/Autocomplete/Autocomplete';
import { KeyboardDatePicker } from '@material-ui/pickers';
import { Link, useHistory } from 'react-router-dom';

import { getCountries } from 'app/store/countrySlice';
import { getLanguages } from 'app/store/languageSlice';
import { getCategories } from 'app/store/categorySlice';
import { getAuthorsWithoutAccount } from 'app/store/authorWithoutAccountSlice';
import { getActiveEditorsByRole, getEditors } from 'app/store/editorSlice';
import { createBook, getBooks } from 'app/store/bookSlice';
import { unwrapResult } from '@reduxjs/toolkit';
import swal from 'sweetalert';
import moment from 'moment';
import { decode } from 'jsonwebtoken';
import { last } from 'lodash';

const StyledLink = styled(Link)`
color: 'white',
textDecoration: 'none'
`;
const NewBook = () => {
    const [selectedDate, setSelectedDate] = useState(
        moment(new Date()).format('YYYY-MM-DD')
    );

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
    const [categoryName, setCategoryName] = useState(null);
    const [languageName, setLanguageName] = useState('');
    const [countryName, setCountryName] = useState('');
    const [authorName, setAuthorName] = useState('');
    const [editorRaisonSocial, setEditorRaisonSocial] = useState('');

    const [Title, setTitle] = useState('');
    const [CoverFile, setCoverFile] = useState('');
    const [errorCoverFile, setErrorCoverFile] = useState('');
    const [helperTextCoverFile, setHelperTextCoverFile] = useState('');
    const [CoverName, setCoverName] = useState('');
    const [coverValue, setCoverValue] = useState('');
    const [PDFFile, setPDFFile] = useState('');
    const [pdfValue, setPDFValue] = useState('');
    const [errorPDFFile, setErrorPDFFile] = useState('');
    const [helperTextPDFFile, setHelperTextPDFFile] = useState('');
    const [PDFName, setPDFName] = useState('');
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
    const [errorCategory, setErrorCategory] = useState(false);
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

    const [helperTextCountryName, setHelperTextCountryName] = useState('');
    const [helperTextLanguageName, setHelperTextLanguageName] = useState('');
    const [helperTextPrice, setHelperTextPrice] = useState('');
    const [bookStatus, setBookStatus] = useState('');
    const [bookStatusName, setBookStatusName] = useState('');
    const [helperTextBookStatus, setHelperTextBookStatus] = useState('');
    const [errorBookStatus, setErrorBookStatus] = useState('');
    const [activateChangeAuthor, setActivateChangeAuthor] = useState(false);
    const [activateChangeEditor, setActivateChangeEditor] = useState(false);
    const [activateCoverFile, setActivateCoverFile] = useState(false);
    const [activatePdfFile, setActivatePdfFile] = useState(false);
    const [enabled, setEnabled] = useState(false);
    const { countries } = useSelector(state => state.country);
    const { languages } = useSelector(state => state.language);
    const { categories } = useSelector(state => state.category);
    const { editors } = useSelector(state => state.editor);
    const { authorsWithoutAccount } = useSelector(
        state => state.authorWithoutAccount
    );
    const { loadingSaveBook } = useSelector(state => state.book);
    const { token } = useSelector(state => state.auth);
    const { history } = useHistory();


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

    const classes = useStyles();
    const photoRef = useRef(null);
    const pdfRef = useRef(null);

    const getRateOnSale = (raisonSocial) => {
        const editor = editors.filter(editor => editor.raisonSocial === raisonSocial)[0];
        return editor && editor.rateOnSale;
    }


    const onChangeCategoryName = (e, value) => {
        if (value) {
            setCategoryName(value.categoryName.fr);
            setErrorCategory(false);
            setHelperTextCategoryName('');
        } else {
            setCategoryName(null);
        }
    };

    const hasOneDecimalPoint = number => {

        if (number.split('').filter(c => c === '.').length == 1 ||
            number.split('').filter(c => c === '.').length == 0) {

            return true;
        } else {
            return false;
        }
    }



    const hasSlash = number => {
        if (number.split('').filter(c => c === '/').length > 0) {

            return true;
        } else {
            return false;
        }
    }



    const onChangeLanguageName = (e, value) => {
        if (value) {
            setLanguageName(value.name);
            setErrorLanguage(false);
            setHelperTextLanguageName('');
        } else {
            setLanguageName('');
        }
    };

    const onChangeCountryName = (e, value) => {
        if (value) {
            setCountryName(value.name);
            setErrorCountry(false);
            setHelperTextCountryName('');
        } else {
            setCountryName('');
        }
    };
    const onChangeAuthorName = (e, value) => {
        if (value) {
            setAuthorName(value.authorName);
            setErrorAuthor(false);
            setHelperTextAuthorName('');
            setActivateChangeAuthor(true);
        } else {
            setAuthorName('');
        }
    };

    const onChangeEditorName = (e, value) => {

        if (value) {
            setEditorRaisonSocial(value.raisonSocial);
            setErrorEditor(false);
            setHelperTextEditorName('');
            setActivateChangeEditor(true);
        } else {
            setEditorRaisonSocial('');
        }
    };

    const onChangeStatus = (e, value) => {
        if (value) {
            setBookStatus(value.id);
            setBookStatusName(value.name);
            setErrorBookStatus(false);
            setHelperTextBookStatus('');
        } else {
            setBookStatusName('');
        }
    };

    const validationTitleError = title => {
        if (title === '') {
            setHelperTextTitle('vous devez entrer un titre du livre');
            setErrorTitle(true);
            setEnabled(false);
        } else {
            setHelperTextTitle('');
            setEnabled(true);
        }
    };
    const onChangeTitle = e => {
        setTitle(e.target.value);
        //setErrorTitle(false);
        setHelperTextTitle('');
        //validationTitleError(e.target.value.trim());
    };

    const validationDescriptionError = description => {
        if (description === '') {
            setHelperTextDescription('vous devez entrez une description');
            setErrorDescription(true);
            setEnabled(false);
        } else if (description.trim().length < 2) {
            setHelperTextDescription(
                'la description doit avoir au moins 2 caractéres'
            );
            setEnabled(false);
            setErrorDescription(true);
        } else {
            setHelperTextDescription('');
            setEnabled(true);
        }
    };
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
            setEnabled(false);
        }
        else if (hasOneDecimalPoint(price) === false || hasSlash(price) === true) {
            setErrorPrice(true);
            setHelperTextPrice('vous devez entrer un format de prix correct');
            setEnabled(false);
        }
        else {
            setEnabled(true);
        }
    };
    const onChangePrice = e => {
        setPrice(e.target.value);
        setErrorPrice(false);
        setHelperTextPrice('');
        validationPriceError(e.target.value);
    };

    const validatePageNumberError = pageNumbers => {
        if (pageNumbers === '') {
            setErrorPageNumbers(true);
            setHelperTextPagesNumber('vous devez entrer un nombre de pages');
            setEnabled(false);
        }

        else if (PageNumbers < 0) {
            setErrorPageNumbers(true);
            setHelperTextPagesNumber(
                'vous ne devez pas entrer un nombre négatif'
            );
            setEnabled(false);
        } else {
            setHelperTextPagesNumber('');
            setEnabled(true);
        }
    };

    const onChangePageNumbers = e => {
        setPageNumbers(e.target.value);
        setErrorPageNumbers(false);
        setHelperTextPagesNumber('');
        validatePageNumberError(e.target.value);
    };

    const validateISBNError = isbn => {
        if (isbn === '') {
            setErrorISBN(true);
            setHelperTextISBN('vous devez entrer un isbn');
            setEnabled(false);
        } else {
            setEnabled(true);
        }
    };

    const onChangeISBN = e => {
        setISBN(e.target.value);
        setErrorISBN(false);
        setHelperTextISBN('');
        validateISBNError(e.target.value);
    };

    const onChangeCover = e => {
        const fileObj = e.target.files && e.target.files[0];
        const fileValue = e.target.value;

        if (!fileObj) {
            setCoverValue(e.target.value);
            return;
        }

        if (e.target.files[0].size / 1024 / 1024 > 40) {
            setErrorCoverFile(true);
            setHelperTextCoverFile('la taille du fichier est très grande');
            photoRef.current.value = null;
        } else {
            setCoverFile(e.target.files[0]);
            setActivateCoverFile(true);

            setCoverName(e.target.files[0].name);
            setErrorCoverFile(false);
            setHelperTextCoverFile('');
        }

    };
    const onChangePdf = e => {
        const fileObj = e.target.files && e.target.files[0];
        if (!fileObj) {
            return;
        }
        const fileSize = fileObj.size / 1024 / 1024; // Convert from bytes to MB
        if (fileSize > 20) {
            setHelperTextPDFFile('la taille du fichier ne doit pas dépasser les 20Mo');
          return;
        }
        setErrorPDFFile(false);
        setPDFFile(e.target.files[0]);


        setPDFName(e.target.files[0].name);
        setHelperTextPDFFile('');
        setActivatePdfFile(true);
    };




    useEffect(() => {
        loadAuthors();
        loadActiveEditors();
        loadCategories();
        loadCountries();
        loadLanguages();
        if (decode(token).exp * 1000 < Date.now()) {
            localStorage.clear();
            history.push('/')
        }
    }, []);

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

    const categoriesParsed = categories.filter(category => category.parentId !== null)
        .map(row => ({
            ...row,
            categoryName: JSON.parse(row['categoryName'])
        }));

    const authorsMapped = authorsWithoutAccount.map(author => ({
        ...author,
        authorName: `${author.firstName} ${author.lastName}`
    }));


    const getCategoryId = name => {
        for (let i = 0; i < categoriesParsed.length; i++) {
            if (categoriesParsed[i].categoryName.fr === name) {
                return categoriesParsed[i].id;
            }
        }
    };
    const getAuthorId = (authorName) => {
        for (let i = 0; i < authorsMapped.length; i++) {
            if (authorsMapped[i].authorName === authorName) {
                return authorsWithoutAccount[i].id;
            }
        }
    };



    const getEditorId = (raisonSocial) => {
        for (let i = 0; i < editors.length; i++) {
            if (
                editors[i].raisonSocial === raisonSocial
            ) {
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
    const reset = () => {
        setTitle('');
        setAuthorName('');
        setActivateChangeEditor(false);
        setDescription('');
        setCategoryName('');
        setCountryName('');
        setPageNumbers('');
        setLanguageName('');
        setISBN('');
        setBookStatusName('');
        setPrice('');
        setSelectedDate(moment(new Date()).format('YYYY-MM-DD'));
        dispatch(getBooks());
        photoRef.current.value = null;
        pdfRef.current.value = null;
        setEnabled(false);
    };

    const addBook = () => {
        if (decode(token).exp * 1000 < Date.now()) {
            localStorage.clear();
            history.push('/')
        }
        let formData = new FormData();
        formData.append('Title', Title);
        formData.append('Description', Description);
        formData.append('PageNumbers', PageNumbers);
        formData.append('PublicationDate', selectedDate);
        formData.append('CoverFile', CoverFile);
        formData.append('PDFFile', PDFFile);
        formData.append('ISBN', ISBN);
        formData.append('AuthorId', getAuthorId(authorName));
        formData.append(
            'PublisherId',
            getEditorId(editorRaisonSocial)
        );
        formData.append('CategoryId', getCategoryId(categoryName));
        formData.append('LanguageId', getLanguageId(languageName));
        formData.append('CountryId', getCountryId(countryName));
        formData.append('Status', bookStatus);
        formData.append('Price', Price.replace('.', ','));

        validationTitleError(Title);
        validationDescriptionError(Description);
        validationPriceError(Price);
        validateISBNError(ISBN);
        validatePageNumberError(PageNumbers);


        if (categoryName === null) {
            setErrorCategory(true);
            setHelperTextCategoryName('vous devez choisir une catégorie');
        }
        if (languageName === '') {
            setErrorLanguage(true);
            setHelperTextLanguageName('vous devez choisir une langue');
        }

        if (authorName === '') {
            setErrorAuthor(true);
            setHelperTextAuthorName('vous devez choisir un auteur');
            setActivateChangeAuthor(false);
        }

        if (CoverFile === '') {
            setErrorCoverFile(true);
            setHelperTextCoverFile('couverture non téléchargé');
        }

        if (PDFFile === '') {
            setActivatePdfFile(false);
        }

        if (bookStatus === '') {
            setErrorBookStatus(true);
            setHelperTextBookStatus("veuillez renseigner l'état du livre");
        }
        if (countryName === '') {
            setErrorCountry(true);
            setHelperTextCountryName("vous devez choisir un pays");
        }
        if (errorCountry === false
            && errorCountry === false
            && errorBookStatus === false
            && errorCategory === false
            && errorLanguage === false
            && errorPrice === false
            && errorISBN === false
            && ((getRateOnSale(editorRaisonSocial) * Price / 100) < 5)
        ) {
            var span = document.createElement("span");
            span.innerHTML = `Le prix actuel calculé est ${Math.ceil(getRateOnSale(editorRaisonSocial) * Price / 100)} DT <br\> Etes vous sur de créer ce livre?`;
            swal({
                title: `Le prix minimal du "${Title}" devrait être 5 DT`,
                content: span,
                icon: 'info',
                buttons: ['Non', 'Oui'],
                dangerMode: true
            }).then((confirm) => {
                if (confirm) {
                    dispatch(createBook(formData))
                        .then(unwrapResult)
                        .then(originalPromiseResult => {
                            swal({
                                title: 'Livre ajouté!',
                                icon: 'success',
                            });

                            reset();
                        });
                }

            });
        } else {
            dispatch(createBook(formData))
                .then(unwrapResult)
                .then(originalPromiseResult => {
                    swal({
                        title: 'Livre ajouté!',
                        icon: 'success',
                    });

                    reset();
                });
        }


    };

    return (
        <FusePageCarded
            header={
                <div className="flex flex-1 justify-between items-center">
                    <div className="pt-10 pb-10">
                        <div className="flex flex-col max-w-full min-w-0 text-white">
                            <Link
                                to="/book/list"
                                className="flex items-center sm:mb-8"
                                style={{
                                    color: '#fff',
                                    textDecoration: 'none'
                                }}
                            >
                                <ArrowBack fontSize="5px" />
                                <b>Livres</b>
                            </Link>
                        </div>
                        <div className="flex items-center max-w-full">
                            <img
                                src={book}
                                alt="image"
                                className="rounded-md"
                            />
                            <div className="flex flex-col mx-8">
                                <h2>
                                    {Title !== '' ? Title : 'Nouveau Livre'}
                                </h2>
                                <p>Détails du livre</p>
                            </div>
                        </div>
                    </div>
                    {
                        loadingSaveBook === true ?
                            <CircularProgress color='#F75454' size={20} />
                            :
                            <Button
                                onClick={addBook}
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
                    <Box>
                        <TextField
                            id="outlined-basic"
                            onChange={onChangeTitle}
                            label="Nom du livre"
                            variant="outlined"
                            className="w-full"
                            name="name"
                            value={Title}
                        //required
                        //error={errorTitle}
                        //helperText={helperTextTitle}
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
                        //error={errorDescription}
                        //helperText={helperTextDescription}
                        //required
                        />
                        <Autocomplete
                            id="combo-box-demo"
                            onChange={onChangeCategoryName}
                            options={categoriesParsed}
                            value={{
                                categoryName: {
                                    fr: categoryName
                                }
                            }}
                            getOptionLabel={option => option.categoryName.fr}
                            className="w-full mt-10"
                            name="category"
                            disablePortal={true}
                            disableClearable
                            required
                            renderInput={params => (
                                <TextField
                                    {...params}
                                    label="Categories"
                                    variant="outlined"
                                    fullWidth
                                    required
                                    error={errorCategory}
                                    helperText={helperTextCategoryName}
                                />
                            )}
                        />
                        {activateChangeAuthor ? (
                            <Autocomplete
                                id="combo-box-demo"
                                onChange={onChangeAuthorName}
                                options={authorsMapped}
                                key={activateChangeAuthor}
                                getOptionLabel={option => option.authorName}
                                value={{
                                    authorName
                                }}
                                noOptionsText="pas d'auteurs"
                                className="w-full mt-10"
                                name="author"
                                disablePortal={true}
                                disableClearable
                                required
                                renderInput={params => (
                                    <TextField
                                        {...params}
                                        label="Nom de l'auteur"
                                        variant="outlined"
                                        fullWidth
                                        required
                                    />
                                )}
                            />
                        ) : (
                            <Autocomplete
                                id="combo-box-demo"
                                onChange={onChangeAuthorName}
                                options={authorsMapped}
                                noOptionsText="pas d'auteurs"
                                getOptionLabel={option => option.authorName || ''}
                                className="w-full mt-10"
                                name="author"
                                disablePortal={true}
                                disableClearable
                                renderInput={params => (
                                    <TextField
                                        {...params}
                                        label="Nom de l'auteur"
                                        variant="outlined"
                                        fullWidth
                                        required
                                        error={errorAuthor}
                                        helperText={helperTextAuthorName}
                                    />
                                )}
                            />
                        )}
                        {activateChangeEditor ? (
                            <Autocomplete
                                id="combo-box-demo"
                                onChange={onChangeEditorName}
                                options={editors}
                                key={activateChangeEditor}
                                value={{
                                    raisonSocial: editorRaisonSocial,
                                }}
                                noOptionsText="pas d'editeurs"
                                getOptionLabel={option => option.raisonSocial || ''}
                                className="w-full mt-10"
                                name="editor"
                                disablePortal={true}
                                disableClearable
                                renderInput={params => (
                                    <TextField
                                        {...params}
                                        label="Maison d'édition"
                                        variant="outlined"
                                        fullWidth
                                    />
                                )}
                            />
                        ) : (
                            <Autocomplete
                                id="combo-box-demo"
                                onChange={onChangeEditorName}
                                options={editors}
                                noOptionsText="pas d'editeurs"
                                getOptionLabel={option => option.raisonSocial || ''}
                                className="w-full mt-10"
                                name="editor"
                                disablePortal={true}
                                disableClearable
                                renderInput={params => (
                                    <TextField
                                        {...params}
                                        label="Maison d'édition"
                                        variant="outlined"
                                        fullWidth
                                    />
                                )}
                            />
                        )}

                        <TextField
                            id="outlined-basic"
                            label="Nombre de pages"
                            variant="outlined"
                            className="w-full mt-10"
                            name="pages"
                            onChange={onChangePageNumbers}
                            value={PageNumbers}
                            required
                            error={errorPageNumbers}
                            helperText={helperTextPagesNumber}
                            onKeyPress={onPress}
                        />
                        <TextField
                            id="outlined-basic"
                            label="ISBN"
                            variant="outlined"
                            className="w-full mt-10"
                            onChange={onChangeISBN}
                            value={ISBN}
                            error={errorISBN}
                            helperText={helperTextISBN}
                        />

                        <Autocomplete
                            id="combo-box-demo"
                            options={countries}
                            getOptionLabel={option => option.name}
                            className="w-full mt-10"
                            name="country"
                            onChange={onChangeCountryName}
                            disablePortal={true}
                            value={{
                                name: countryName
                            }}
                            disableClearable
                            renderInput={params => (
                                <TextField
                                    {...params}
                                    label="Pays"
                                    variant="outlined"
                                    fullWidth
                                    required
                                    error={errorCountry}
                                    helperText={helperTextCountryName}
                                />
                            )}
                        />
                        <Autocomplete
                            id="combo-box-demo"
                            options={languages}
                            onChange={onChangeLanguageName}
                            getOptionLabel={option => option.name}
                            className="w-full mt-10"
                            name="Langues"
                            disablePortal={true}
                            value={{
                                name: languageName
                            }}
                            required
                            disableClearable
                            renderInput={params => (
                                <TextField
                                    {...params}
                                    label="Langues"
                                    variant="outlined"
                                    fullWidth
                                    required
                                    error={errorLanguage}
                                    helperText={helperTextLanguageName}
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
                            minDate={"870-01-01"}
                            maxDate={"3000-01-01"}
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

                            <TextField
                                id="outlined-basic"
                                variant="outlined"
                                className="w-full mt-10"
                                type="file"
                                onChange={onChangePdf}
                                name="filePath"
                                required
                                helperText={helperTextPDFFile}
                                inputRef={pdfRef}
                            />


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
                                    required
                                    inputRef={photoRef}
                                    error={errorCoverFile}
                                    helperText={helperTextCoverFile}
                                />

                            </div>
                        </p>
                        <Autocomplete
                            id="combo-box-demo"
                            onChange={onChangeStatus}
                            options={status}
                            getOptionLabel={option => option.name}
                            className="w-full mt-10"
                            name="bookStatus"
                            value={{
                                name: bookStatusName
                            }}

                            disableClearable
                            renderInput={params => (
                                <TextField
                                    {...params}
                                    label="Etat du livre"
                                    variant="outlined"
                                    fullWidth
                                    error={errorBookStatus}
                                    helperText={helperTextBookStatus}
                                />
                            )}
                        />

                    </Box>
                </TabPanel>
            }
        />
    );
};

export default NewBook;
