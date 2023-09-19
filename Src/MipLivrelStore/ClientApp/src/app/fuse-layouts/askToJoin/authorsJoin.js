import React, { useState, useEffect } from 'react';
import FuseCountdown from '@fuse/core/FuseCountdown';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { Box, Button, InputAdornment, TextField, Modal, Typography } from '@material-ui/core';
import { PersonAdd } from '@material-ui/icons';
import { Link } from 'react-router-dom';
import { DataGrid } from '@material-ui/data-grid';
import { makeStyles, createStyles } from '@material-ui/styles';
import '../styles.css';
import UserRow from '../shared-components/UserRow';
import swal from 'sweetalert';
import { useHistory } from 'react-router-dom';
import { KeyboardDatePicker } from '@material-ui/pickers';

import { useDispatch, useSelector } from 'react-redux';
import { getAuthorsToJoin, updateJoinRequest, reset, setDateDebut, setDateFin } from '../../store/authorJoinSlice';
import { getCountries } from '../../store/countrySlice';
import moment from 'moment';
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
		'& .MuiDataGrid-columnsContainer': {
			borderTopLeftRadius: '20px',
			borderTopRightRadius: '20px'
		},
		borderTopLeftRadius: "20px",
		borderTopRightRadius: "20px"
	}
}));
const style = {
	position: 'absolute',
	top: '50%',
	left: '50%',
	transform: 'translate(-50%, -50%)',
	width: 530,
	backgroundColor: '#fff',
	boxShadow: 24,
	height: 'auto',
	borderRadius: '25px'
};

const headerStyle = {
	backgroundColor: 'rgb(37,47,62)',
	height: 'auto',
	width: '100%',
	color: '#fff',
	padding: '10px 30px',
	borderTopRightRadius: '25px',
	borderTopLeftRadius: '25px'
};
const AuthorsJoin = () => {
	const classes = useStyles();
	const history = useHistory();
	const [author, setAuthor] = useState('');
	const [open, setOpen] = useState(false);
	const handleClose = () => setOpen(false);
	const [selectedStartDate, setSelectedStartDate] = useState(moment(new Date()).format('YYYY-MM-DD'));
	const [selectedEndDate, setSelectedEndDate] = useState(moment(new Date()).format('YYYY-MM-DD'));

	const [authors, setAuthors] = useState([]);
	const dispatch = useDispatch();
	const { authorsJoined, dateDebut, dateFin } = useSelector(state => state.authorJoin);
	const { countries } = useSelector(state => state.country);
	const regex = /\//g;
	let dated = "";
	let datef = "";
	const {token} = useSelector(state => state.auth);
	useEffect(() => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }
		dispatch(reset());
		dispatch(getCountries());
		if (dateDebut === "" && dateFin === "") {
			dispatch(getAuthorsToJoin({ FromDate: selectedStartDate.toString().replace(regex, '-'), ToDate: selectedEndDate.toString().replace(regex, '-'), RequesterType: 1 }));
		}
	}, []);

	const onChangeStartDate = (e, row) => {
		console.log(dateDebut, dateFin)
		if (dateFin === "") {
			setSelectedStartDate(row);
			dispatch(getAuthorsToJoin({ FromDate: row.toString().replace(regex, '-'), ToDate: selectedEndDate.toString().replace(regex, '-'), RequesterType: 1 }));
		} else {
			setSelectedStartDate(row);
		}
	};
	const onChangeEndDate = async (e, row) => {
		setSelectedEndDate(row);
		dated = selectedStartDate.toString().replace(regex, '-');
		datef = row.toString().replace(regex, '-');
		dispatch(setDateDebut(dated));
		dispatch(setDateFin(datef));
		dispatch(getAuthorsToJoin({ FromDate: dated, ToDate: datef, RequesterType: 1 }));
	};

	const getCountryName = countryId => {
		const obj = countries.filter(country => country.id === countryId)[0];
		if (obj !== undefined) {
			return obj.name;
		}
	};


	const columns = [
		{
			field: 'firstName',
			headerName: 'Nom',
			headerClassName: classes.hideRightSeparator,
			flex: 1
		},
		{
			field: 'lastName',
			headerName: 'Prenom',
			headerClassName: classes.hideRightSeparator,
			flex: 1
		},
		{
			field: 'email',
			headerName: 'Email',

			headerClassName: classes.hideRightSeparator,
			width: 200
		},
		{
			field: 'phoneNumber',
			headerName: 'Telephone',
			headerClassName: classes.hideRightSeparator,
			flex: 1
		},
		{
			field: 'countryId',
			headerName: 'Pays',
			headerClassName: classes.hideRightSeparator,
			flex: 1,
			renderCell: params => {
				return <span>{getCountryName(params.value)}</span>;
			}
		},
		{
			field: 'status',
			headerName: 'Etat',
			flex: 1,
			renderCell: params => {
				if (params.value === 3) {
					return (
						<div className="bg-purple-700 text-white  py-4 px-12 rounded-full center h-28">En Attente</div>
					);
				} else if (params.value === 2) {
					return <div className="bg-red-700 text-white  py-4 px-12 rounded-full center h-28">Rejeté</div>;
				} else if (params.value === 1) {
					return <div className="bg-green-700 text-white  py-4 px-12 rounded-full center h-28">Validé</div>;
				} else {
					return (
						<div className="bg-purple-700 text-white  py-4 px-12 rounded-full center h-28">En Attente</div>
					);
				}
			}
		},
		{
			field: 'actions',
			headerName: 'Actions',
			flex: 1,
			headerClassName: classes.hideRightSeparator,
			disableClickEventBubbling: true,
			renderCell: params => {
				const onDeleteClick = e => {
					e.stopPropagation();
					const index = authorsJoined.findIndex(c => c.id == params.row.id);
					swal({
						title: "Refuser l'auteur",
						text: 'Etes vous sur ?',
						icon: 'error',
						buttons: ['Non', 'Oui'],
						dangerMode: true,
						timer: 2000
					}).then(isConfirm => {
						if (isConfirm) {
							dispatch(
								updateJoinRequest({
									...authorsJoined[index],
									status: 2
								})
							).then(originalPromiseResult => {
								swal({
									text: "Demande d'adhésion rejetée",
									type: 'success',
									icon: 'success',
									confirmButtonText: 'Ok'
								});
								dispatch(setDateDebut(dated));
								dispatch(setDateFin(datef));
								dispatch(getAuthorsToJoin({ FromDate: dateDebut, ToDate: dateFin, RequesterType: 1 }));
							});
						} else {
							return false;
						}
					});
				};
				const onValidateClick = e => {
					e.stopPropagation();
					const index = authorsJoined.findIndex(c => c.id == params.row.id);
					swal({
						title: "Accepter l'auteur",
						text: 'Etes vous sur ?',
						icon: 'success',
						buttons: ['Non', 'Oui'],
						dangerMode: true,
						timer: 2000
					}).then(isConfirm => {
						if (isConfirm) {
							dispatch(
								updateJoinRequest({
									...authorsJoined[index],
									status: 1
								})
							).then(originalPromiseResult => {
								swal({
									text: "Demande d'adhésion Acceptée",
									type: 'success',
									icon: 'success',
									confirmButtonText: 'Ok'
								});
								dispatch(setDateDebut(dated));
								dispatch(setDateFin(datef));
								dispatch(getAuthorsToJoin({ FromDate: dateDebut, ToDate: dateFin, RequesterType: 1 }));
							});
						} else {
							return false;
						}
					});

				};
				const onShowDetails = e => {
					setOpen(true);
					setAuthor(params.row);
				};

				return (
					<UserRow
						index={params.row.id}
						validate={onValidateClick}
						cancel={onDeleteClick}
						showDetails={onShowDetails}
					/>
				);
			}
		}
	];
	console.log(author);
	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center w-full">
					<div className="pt-10 pb-10">
						<div className="flex items-center">
							<PersonAdd className="text-32" />
							<span className="ml-8 text-16 md:text-24 font-semiblod mt-4">
								<b>Auteurs</b>
							</span>
						</div>
						<div className="flex flex-row">
							<KeyboardDatePicker
								autoOk
								variant="inline"
								inputVariant="outlined"
								label="Date de début"
								value={selectedStartDate}
								format="YYYY/MM/DD"
								onChange={onChangeStartDate}
								className="w-full mt-10"
							/>

							<KeyboardDatePicker
								autoOk
								variant="inline"
								inputVariant="outlined"
								label="Date de Fin"
								value={selectedEndDate}
								format="YYYY/MM/DD"
								onChange={onChangeEndDate}
								className="w-full mt-10 ml-8"
							/>
						</div>
					</div>
				</div>
			}
			content={
				<div className="rounded-t-20 w-full h-full">
					<Modal
						open={open}
						onClose={handleClose}
						aria-labelledby="modal-modal-title"
						aria-describedby="modal-modal-description"
					>
						<Box style={style}>

							<div
								className="p-20 flex-col"
							>
								<div className="flex flex-row mb-10">
									<div className="flex-col flex-1 pl-10 mb-2">
										<Typography
											id="modal-modal-title"
											variant="h6"
											component="h2"
											className="text-15 mb-10"
										>
											<b>Nom:</b>{' '}
											{author && author['firstName']}
										</Typography>

										<Typography
											id="modal-modal-title"
											variant="h6"
											component="h2"
											className="text-15"
										>
											<b>Email</b>:{' '}
											{author && author['email']}
										</Typography>
									</div>
									<div className="flex flex-col pl-10 flex-1">
										<Typography
											id="modal-modal-title"
											variant="h6"
											component="h2"
											className="text-15 mb-10"
										>
											<b>Prénom:</b>{' '}
											{author && author['lastName']}
										</Typography>

										<Typography
											id="modal-modal-title"
											variant="h6"
											component="h2"
											className="text-15"
										>
											<b>Pays:</b>{' '}
											{author && getCountryName(author['countryId'])}
										</Typography>
									</div>
								</div>

								<div className="flex-row">
									<Typography
										id="modal-modal-title"
										variant="h6"
										component="h2"
										className="text-15 ml-10"
									>
										<b>Biography:</b>{' '}
										{author && author['description']}
									</Typography>
								</div>
							</div>
						</Box>
					</Modal>
					<DataGrid
						className={classes.root}
						disableSelectionOnClick
						rows={authorsJoined}
						columns={columns}
						disableColumnMenu={true}
						pageSize={5}
						rowsPerPageOptions={[10]}
					/>
				</div>
			}
		/>
	);
};

export default AuthorsJoin;
