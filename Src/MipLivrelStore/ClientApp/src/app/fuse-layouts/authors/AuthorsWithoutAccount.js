import React, { useState, useEffect } from 'react';
import FuseCountdown from '@fuse/core/FuseCountdown';
import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	Box,
	Button,
	Typography
} from '@material-ui/core';
import {
	PersonAddDisabled,
	Add,
	Person
} from '@material-ui/icons';
import { Link } from 'react-router-dom';
import { DataGrid } from '@material-ui/data-grid';
import { makeStyles, createStyles } from '@material-ui/styles';
import '../styles.css';
import UserWithoutAccountRow from '../shared-components/UserWithoutAccountRow';
import { useHistory } from 'react-router-dom';
import Modal from '@material-ui/core/Modal';
import moment from 'moment';
import { deleteAuthorWithoutAccount, getAuthorsWithoutAccount } from 'app/store/authorWithoutAccountSlice';
import { useDispatch, useSelector } from 'react-redux';
import { getCountries } from 'app/store/countrySlice';
import swal from 'sweetalert';
import { decode } from 'jsonwebtoken';
import ExportExcel from '../shared-components/ExcelExport';

const useStyles = makeStyles(theme => ({
	modal: {
		display: 'flex',
		alignItems: 'center',
		justifyContent: 'center'
	},
	paper: {
		position: 'absolute',
		width: 450,
		backgroundColor: theme.palette.background.paper,
		boxShadow: theme.shadows[5],
		padding: theme.spacing(2, 4, 3)
	},
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
		borderTopLeftRadius: '20px',
		borderTopRightRadius: '20px'
	}
}));

const AuthorsWithoutAccount = () => {
	const [open, setOpen] = useState(false);
	const handleClose = () => setOpen(false);
	const [author, setAuthor] = useState(null);
	const dispatch = useDispatch();
	const { authorsWithoutAccount, loading } = useSelector(
		state => state.authorWithoutAccount
	);
	const { countries } = useSelector(state => state.country);
	const [authorWithoutAccountId, setAuthorWithoutAccountId] = useState('');
	const {token} = useSelector(state => state.auth);
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
	const detailsHeaderStyle = {
		backgroundColor: 'rgb(37,47,62)',
		height: 'auto',
		width: '100%',
		color: '#fff',
		padding: '10px 30px',
		borderTopRightRadius: '25px',
		borderTopLeftRadius: '25px',
		display: 'flex',
		alignItems: 'center',


	};

	const tableStyle = {
		borderTopLeftRadius: '20px',
		borderTopRightRadius: '20px',
		width: '100%',
		height: '100%'
	};
	useEffect(() => {
		dispatch(getAuthorsWithoutAccount());
		dispatch(getCountries());
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
		}
	}, []);
	const rows = authorsWithoutAccount.map(author => ({
		...author,
		birthdate: moment(author.birthdate).format('YYYY-MM-DD'),
		inscriptionDate: '14-05-2010'
	}));

	const classes = useStyles();
	const history = useHistory();
	const getCountryName = (id) => {
		const obj = countries.filter(
			country => country.id === id
		)[0];
		if (obj !== undefined) {
			return obj.name;
		}
	};
	const ExcelExportData = authorsWithoutAccount.map(author => ({
		'phoneNumber': author.phoneNumber,
		'firstName': author.firstName,
		'lastName': author.lastName,
		'birdhdate': moment(author.birthdate).format('YYYY-MM-DD'),
		'email': author.email,
		'address:': author.address,
		'countryName': getCountryName(author.countryId),
		'Biography': author.biography
	}));
	const columns = [
		{
			field: '',
			headerName: '',
			headerAlign: 'center',
			sortable: false,
			width: 80,
			headerClassName: classes.hideRightSeparator,
			renderCell: params => {
				return (
					<div className="rounded p-3">
						<img
							src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${params.row.photoPath}`}
							style={{
								borderRadius: '50%'
							}}
						/>
					</div>
				);
			}
		},
		{
			field: 'firstName',
			headerName: 'Prénom',
			headerClassName: classes.hideRightSeparator,
			flex: 2
		},
		{
			field: 'lastName',
			headerName: 'Nom',
			headerClassName: classes.hideRightSeparator,
			flex: 2
		},
		{
			field: 'email',
			headerName: 'E-mail',
			headerClassName: classes.hideRightSeparator,
			flex: 2,
			headerAlign:"center",
			align:"center"
		},
		{
			field: 'birthdate',
			headerName: 'Date de naissance',
			headerClassName: classes.hideRightSeparator,
			flex: 2,
			headerAlign:'center',
			align:'center'
		},
		{
			field: 'actions',
			headerName: 'Actions',
			width: 170,
			flex: 1,
			headerClassName: classes.hideRightSeparator,
			disableClickEventBubbling: true,
			renderCell: params => {
				const onShowDetails = e => {
					setOpen(true);
					const obj = rows.filter(c => c.id == params.row.id)[0];
					setAuthor(params.row);
				};

				const onUpdateClick = e => {
					history.push(`/authorWithoutAccount/list/${params.row.id}`);
				};
				const onDeleteClick = e => {
				
					swal({
						title: "Supprimer l'auteur",
						text: 'Etes vous sur ?',
						icon: 'error',
						buttons: ['Non', 'Oui'],
						dangerMode: true,
			
					}).then(confirm => {
						if(confirm) {
							dispatch(deleteAuthorWithoutAccount(params.row.id)).then(() => {
								swal({
									title: 'Auteur Supprimé!',
									icon: 'success'
								});
								dispatch(getAuthorsWithoutAccount())
							})
						}
					})
				
			}
				return (
					<UserWithoutAccountRow
						index={params.row.id}
						showDetails={onShowDetails}
						update={onUpdateClick}
						remove={onDeleteClick}
					/>
				);
			}
		}
	];
	

	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center w-full">
					<div className="pt-10 pb-10">
						<div className="flex items-center">
							<Person className="text-32" />
							<span className="ml-8 text-16 md:text-24 font-semiblod mt-4">
								<b>Auteurs via ME</b>
							</span>
							
						</div>
					</div>
					<div className='flex items-center'>
					<Link
						to="/authorWithoutAccount/list/new"
						style={{ textDecoration: 'none' }}
					>
						<Button
							size="small"
							variant="container"
							className="save-btn"
							color="primary"
						>
							<Add className="mr-8" />
							Ajouter
						</Button>
					</Link>
					<ExportExcel excelData={ExcelExportData} fileName={"Authors via ME"}/>
			   	</div>
				</div>
			}
			content={
				<div style={tableStyle}>
					<Modal
						open={open}
						onClose={handleClose}
						aria-labelledby="modal-modal-title"
						aria-describedby="modal-modal-description"
					>
						<Box style={style}>
							<div style={detailsHeaderStyle}>
								<img
									src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${author && author.photoPath}`}
									alt="user"
									className="w-32 sm:w-48 rounded mr-5"
									width={100}
									height={100}
								/>
								<div style={{
									display: 'flex',
									flexDirection: 'column'
								}}>
									<Typography
										id="modal-modal-title"
										variant="h6"
										component="h2"
										className="text-15"
									>
										Détails de l'auteur
								</Typography>
									<span className="mt-5">{`${author && author['firstName']} ${author && author['lastName']}`} </span>
								</div>
							</div>
							<div /*style={detailsBodyStyle}*/
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
											<b>Date:</b>{' '}
											{author && author['birthdate']}
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
										<b>Pays:</b>{' '}
										{author &&
											getCountryName(author['countryId'])}
									</Typography>
								</div>
							</div>
						</Box>
					</Modal>
					<DataGrid
						className={classes.root}
						rows={rows}
						columns={columns}
						pageSize={10}
						rowsPerPageOptions={[5, 10, 20]}
						onSelectionChange={newSelection => {
							setAuthorWithoutAccountId(newSelection.rowIds[0]);
						}}
					
						options={{
							draggable: false
						}}
					/>
				</div>
			}
		/>
	);
};

export default AuthorsWithoutAccount;
