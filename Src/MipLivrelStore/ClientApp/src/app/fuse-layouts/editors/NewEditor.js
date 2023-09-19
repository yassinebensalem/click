import FusePageCarded from '@fuse/core/FusePageCarded';
import {
    AppBar,
    Box,
    Button,
    CircularProgress,
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
import { Visibility, VisibilityOff, Phone, LastPage } from '@material-ui/icons';
import { useDispatch } from 'react-redux';
import { updateEditor, getEditors, getActiveEditorsByRole, createEditor } from 'app/store/editorSlice';
import { unwrapResult } from '@reduxjs/toolkit';
import { getCountries } from 'app/store/countrySlice';
import { useSelector } from 'react-redux';
import { decode } from 'jsonwebtoken';

const NewEditor = ({ match }) => {
    const dispatch = useDispatch();
    const [author, setAuthor] = useState(null);
    const [value, setValue] = useState(0);
    const classes = useStyles();
    const [IdFiscal, setIdFiscal] = useState('');
    const [RaisonSocial, setRaisonSocial] = useState('');
    const [civilite, setCivilite] = useState('');
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [password, setPassword] = useState('');

    const [disabled, setDisabled] = useState(true);

    const [address, setAddress] = useState('');
    const [email, setEmail] = useState('');
    const [photoPath, setPhotoPath] = useState('');
    const [countryId, setCountryId] = useState('');
    const [ratioPrice, setRatioPrice] = useState('');
    const [ratioPriceError, setRatioPriceError] = useState(false);
    const [helperTextRatioPrice, setHelperTextRatioPrice] = useState('');
    const [ratioSelling, setRatioSelling] = useState('');
    const [ratioSellingError, setRatioSellingError] = useState(false);
    const [helperTextRatioSelling, setHelperTextRatioSelling] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [editorId, setEditorId] = useState('');
    const [photoUpdated, setPhotoUpdated] = useState('');
    const [phone, setPhone] = useState('');
    const [isActive, setIsActive] = useState(false);

    const { countries } = useSelector(state => state.country);

    const [enabled, setEnabled] = useState(false);
    const photoRef = useRef(null);
    const { loadingUpdateEditor } = useSelector(state => state.editor);
    const { token } = useSelector(state => state.auth);
    const { history } = useHistory();
    useEffect(() => {
        let estAffiche = true;
        if (decode(token).exp * 1000 < Date.now()) {
            localStorage.clear();
            history.push('/')
        }
        dispatch(getCountries());
        dispatch(getEditors())
            .then(unwrapResult)
            .then(res => {

                const obj = res.filter(obj => obj.id === match.params.id)[0];
                console.log(obj);
                if (obj && estAffiche) {
                    setRaisonSocial(obj.raisonSocial);
                    setIdFiscal(obj.idFiscal);
                    setEmail(obj.email);
                    setAddress(obj.address);
                    setCountryId(obj.countryId);
                    setCivilite(obj.gender);
                    setSelectedDate(obj.birthdate);
                    setEditorId(obj.id);
                    setRatioPrice(obj.rateOnOriginalPrice);
                    setRatioSelling(obj.rateOnSale);
                    setPhotoPath(obj.photoPath);
                    setPhone(obj.phoneNumber);
                    setFirstName(obj.firstName);
                    setLastName(obj.lastName);
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
    const onChangeCountryId = (e, value) => {
        setCountryId(value.id);
        if (password === undefined) {
            setEnabled(true);
        } else {
            setEnabled(false)
        }
    };
    const hasSlash = number => {
        if (number.split('').filter(c => c === '/').length > 0) {

            return true;
        } else {
            return false;
        }
    }

    const hasOneDecimalPoint = number => {

        if (number.split('').filter(c => c === '.').length == 1 ||
            number.split('').filter(c => c === '.').length == 0) {

            return true;
        } else {
            return false;
        }
    }

    const validateRatioPriceError = ratioPrice => {

        if (ratioPrice === '') {
            setRatioPriceError(true);
            setHelperTextRatioPrice('vous devez entrer un prix par ratio');
            setEnabled(false);

        }
        else if (ratioPrice > 100 || ratioPrice < 0) {
            setRatioPriceError(true);
            setHelperTextRatioPrice('Le nombre devrait être compris entre 0 et 100');
            setEnabled(false);
        }
        else {
            setEnabled(true);
        }
    };

    const validateRatioSellingError = ratioSelling => {
        if (ratioSelling === '') {
            setRatioSellingError(true);
            setHelperTextRatioSelling('vous devez entrer un prix par achat');
            setEnabled(false);
        }
        else if (ratioSelling > 100 || ratioSelling < 0) {
            setRatioSellingError(true);
            setHelperTextRatioSelling('Le nombre devrait être compris entre 0 et 100');
            setEnabled(false);
        }
        else if (hasSlash(ratioSelling) === true) {
            setRatioSellingError(true);
            setHelperTextRatioSelling('vous devez entrer un format de ratio correct');
            setEnabled(false);
        }
        else {
            setEnabled(true);
        }
    };


    const addEditor = () => {
        if (decode(token).exp * 1000 < Date.now()) {
            localStorage.clear();
            history.push('/')
        }
        let formData = new FormData();
        formData.append('Id', editorId || '');
        formData.append('RaisonSocial', RaisonSocial || '');
        formData.append('IdFiscal', IdFiscal || '');
        formData.append('CountryId', getCountryId(getCountryName()) || '');
        formData.append('PhotoFile', photoUpdated || '');
        formData.append('Email', email || '');
        formData.append('PhoneNumber', phone || '');
        formData.append('Address', address || '');
        formData.append('RateOnOriginalPrice', ratioPrice.toString().replace('.', ',') || '');
        formData.append('RateOnSale', ratioSelling.toString().replace('.', ',') || '');
        formData.append('FirstName', firstName || '');
        formData.append('LastName', lastName || '');
        formData.append('isActive', isActive);
        formData.append('UserRole', 4);
        dispatch(createEditor(formData))
            .then(unwrapResult)
            .then(originalPromiseResult => {
                swal({
                    title: 'Editeur Ajouté!',
                    icon: 'success'
                });
                validateRatioPriceError(ratioPrice);
                validateRatioSellingError(ratioSelling);
            })
            .catch(rejectedValueOrSerializedError => {
                // handle result here

            });
    };
    const getCountryName = () => {
        const obj = countries.filter(country => country.id === countryId)[0];
        if (obj !== undefined) {
            return obj.name;
        }
    };

    const onChangeRaisonSocial = e => {
        setRaisonSocial(e.target.value);
        setEnabled(true);

    };

    const onChangePhotoPath = e => {
        const fileObj = e.target.files && e.target.files[0];
        if (!fileObj) {
            return;
        }
        setPhotoUpdated(e.target.files[0]);
        setEnabled(true)

    };



    const onChangeEmail = e => {
        setEmail(e.target.value);
    };
    const onChangeAddress = e => {
        setAddress(e.target.value);
        setEnabled(true);
    };

    const onChangeIdFiscal = e => {
        setIdFiscal(e.target.value);
        setEnabled(true);
    };



    const onChangeRatioPrice = e => {
        setRatioPrice(e.target.value);
        setRatioPriceError(false);
        setHelperTextRatioPrice('');
        setEnabled(true);
        validateRatioPriceError(e.target.value.trim());
    };

    const onChangeRatioSelling = e => {
        setRatioSelling(e.target.value);
        setRatioSellingError(false);
        setHelperTextRatioSelling('');
        setEnabled(true);
        validateRatioSellingError(e.target.value.trim());
    };

    const handleChangeTab = (e, newValue) => {
        setValue(newValue);
    };

    const onChangePhone = e => {
        setPhone(e.target.value);
        setEnabled(true)
    };

    const onPress = evt => {
        if (
            (evt.which != 8 && evt.which != 0 && ((evt.which < 45) || evt.which > 57)) ||
            evt.target.value.length > 7

        ) {
            evt.preventDefault();
        }
    };
    const onChangeFirstName = e => {
        setFirstName(e.target.value);
        setEnabled(true);
    }

    const onChangeLastName = e => {
        setLastName(e.target.value);
        setEnabled(true);
    }

    return (
        <FusePageCarded
            header={
                <div className="flex flex-1 justify-between items-center">
                    <div className="pt-10 pb-10">
                        <div className="flex flex-col max-w-full min-w-0">
                            <Link
                                to="/editor/list"
                                className="flex items-center sm:mb-8"
                                style={{
                                    color: 'white',
                                    textDecoration: 'none'
                                }}
                            >
                                <ArrowBack fontSize="5px" />
                                <b className="hidden sm:flex mx-4 font-medium">
                                    Editeurs
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
                                    {RaisonSocial}
                                </h2>{' '}
                                <br />
                                <span
                                    style={{
                                        marginTop: '-10px',
                                        fontSize: '12px'
                                    }}
                                >
                                    Détails de l'editeur
                                </span>
                            </div>
                        </div>
                    </div>
                    {
                        loadingUpdateEditor ?
                            <CircularProgress color='#F75454' size={20} />
                            :
                            <Button
                                onClick={addEditor}
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
                                value={firstName}
                                label="Nom"
                                placeholder="Nom"
                                variant="outlined"
                                className="w-full mb-10"
                            />
                            <TextField
                                id="outlined-basic"
                                onChange={onChangeLastName}
                                value={lastName}
                                label="Prénom"
                                placeholder="Prénom"
                                variant="outlined"
                                className="w-full mb-10"
                            />
                            <TextField
                                id="outlined-basic"
                                label="Raison Social"
                                placeholder="Raison Social"
                                variant="outlined"
                                className="w-full mb-10"
                                value={RaisonSocial}
                                onChange={onChangeRaisonSocial}
                            />
                            <TextField
                                id="outlined-basic"
                                label="Id fiscal"
                                placeholder="Id fiscal"
                                variant="outlined"
                                className="w-full mb-10"
                                value={IdFiscal}
                                onChange={onChangeIdFiscal}
                            />


                            <TextField
                                id="outlined-basic"
                                onChange={onChangeEmail}
                                value={email}
                                label="Adresse -mail"
                                variant="outlined"
                                className="w-full mt-10"
                                defaultValue="Email"
                            />
                            <TextField
                                id="outlined-basic"
                                label="Numéro de téléphone"
                                variant="outlined"
                                className="w-full mt-10"
                                onChange={onChangePhone}
                                onKeyPress={onPress}
                                value={phone}
                            />
                            <Autocomplete
                                id="combo-box-demo"
                                options={countries}
                                getOptionLabel={option => option.name}
                                value={{ name: getCountryName() }}
                                disablePortal={true}
                                onChange={(event, value) => {
                                    setCountryId(value.id);
                                    setEnabled(true);
                                }

                                }
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
                                label="adresse"
                                value={address}
                            />
                            <TextField
                                id="outlined-basic"
                                label="Pourcentage de prix de vente (en %)"
                                placeholder="Pourcentage de prix de vente (en %)"
                                variant="outlined"
                                className="w-full mt-10"
                                onChange={onChangeRatioPrice}
                                value={ratioPrice}
                                onKeyPress={onPress}
                                error={ratioPriceError}
                                helperText={helperTextRatioPrice}
                                required
                            />
                            <TextField
                                id="outlined-basic"
                                label="Marge de Miplivrel (en %)"
                                placeholder="Marge de Miplivrel (en %)"
                                variant="outlined"
                                className="w-full mt-10"
                                onChange={onChangeRatioSelling}
                                value={ratioSelling}
                                onKeyPress={onPress}
                                error={ratioSellingError}
                                helperText={helperTextRatioSelling}
                                required
                            />

                        </Box>
                    </TabPanel>

                </>
            }
        />
    );
};

export default NewEditor;
