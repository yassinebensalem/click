import React, { useState, useEffect } from 'react';
import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	Button,
} from '@material-ui/core';
import {
	Person,
	Add
} from '@material-ui/icons';
import { Link } from 'react-router-dom';
import { DataGrid } from '@material-ui/data-grid';
import { makeStyles, createStyles } from '@material-ui/styles';
import '../styles.css';
import moment from 'moment';
import { useHistory } from 'react-router-dom';
import swal from 'sweetalert';
import { useDispatch, useSelector } from 'react-redux';
import { getAuthors, updateAuthor, deleteAuthor, getActiveAuthorsByRole } from 'app/store/authorSlice';
import { getCountries } from 'app/store/countrySlice';
import UserRoleRow from '../shared-components/UserRoleRow';
import SwitchRowAuthor from '../shared-components/SwitchRowAuthor';
import Search from '../shared-components/Search';
import { sendMail } from 'app/store/authorSlice';
import ExportExcel from '../shared-components/ExcelExport';
import { decode } from 'jsonwebtoken';
import { logout } from 'app/store/authSlice';
import Loading from '../shared-components/Loading';
import { checkToken } from 'app/services/Helpers';

const useStyles = makeStyles(theme =>
	createStyles({
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
	})
);

const Authors = () => {
	const style = {
		position: 'absolute',
		top: '50%',
		left: '50%',
		transform: 'translate(-50%, -50%)',
		width: 530,
		backgroundColor: '#fff',
		boxShadow: 24,
		height: 'auto'
	};
	const history = useHistory();
	const classes = useStyles();
	const [authorId, setAuthorId] = useState('');
	const [isActive, setIsActive] = useState(false);
	const [author, setAuthor] = useState('');
	const [searchQuery, setSearchQuery] = useState('');
	const [email, setEmail] = useState('');
	const [emailConfirmed, setEmailConfirmed] = useState('');

	const dispatch = useDispatch();

	const { authors, isUpdated , loading} = useSelector(state => state.author);
	const {refreshToken, token} = useSelector(state => state.auth);
	const {countries} = useSelector(state => state.country);


	const tableStyle = {
		borderTopLeftRadius: '20px',
		borderTopRightRadius: '20px',
		width: '100%',
		height: '100%'
	};
	const handleClick = () => {
		setAuthor(authors.filter(author => author.id === authorId)[0]);
	};

	const filterData = (data, query) => {
		if (!query) {
			return data;
		} else {
			return data.filter(
				d =>
					(d.firstName || '')
						.toLowerCase()
						.includes((query || '').toLowerCase()) ||
					d.lastName
						.toLowerCase()
						.includes((query || '').toLowerCase())
			);
		}}
		const getCountryName = (id) => {
			const obj = countries.filter(country => country.id === id)[0];
			if (obj !== undefined) {
				return obj.name;
			}
		};
		const ExcelExportData = authors.map(author => ({
			
			'phoneNumber': author.phoneNumber,
			'firstName': author.firstName,
			'lastName': author.lastName,
			'birdhdate': moment(author.birthdate).format('YYYY-MM-DD'),
			'inscriptionDate': moment(author.createdAt).format('YYYY-MM-DD'),
			'email': author.email,
			'address:': author.address,
			'countryName': getCountryName(author.countryId),
			'gender': author.gender
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
			width: 150,
			editable: true,
			headerAlign: 'center',
			align: 'center'
		},
		{
			field: 'lastName',
			headerName: 'Nom',
			headerClassName: classes.hideRightSeparator,
			width: 150,
			editable: true,
			headerAlign: 'center',
			align: 'center'
		},
		{
			field: 'birthdate',
			headerName: 'Date De Naissance',
			width: 170,
			headerClassName: classes.hideRightSeparator,

			headerAlign: 'center',
			align: 'center'
		},
		{
			field: 'inscriptionDate',
			headerName: 'Date D\'inscription',
			width: 170,
			headerClassName: classes.hideRightSeparator,
			headerAlign: 'center',
			align: 'center'
		},
		{
			field: 'booksNumber',
			headerName: 'Nombre de livres',
			width: 170,
			headerClassName: classes.hideRightSeparator,
			headerAlign: 'center',
			align: 'center'
		},
		{
			field: 'raisonSocial',
			headerName: "Maison d'édition",
			width: 170,
			headerClassName: classes.hideRightSeparator,
			renderCell: params => {
				if (params.row.raisonSocial === null) {
					return <span>Auto Finacement</span>;
				}
			}
		},
		{
			field: 'Status',
			headerName: 'Etat Du Compte',
			width: 190,
			headerClassName: classes.hideRightSeparator,
			align: 'center',
			renderCell: params => {
				return (
					<SwitchRowAuthor
						handleClick={handleClick}
						authorId={params.row.id}
						author={
							authors.filter(
								author => author.id === params.row.id
							)[0]
						}
						isActive={
							authors.filter(
								author => author.id === params.row.id
							)[0].isActive
						}
				
					/>
				);
			}
		},
		{
			field: 'actions',
			headerName: 'Actions',
            headerAlign:'center',
			headerClassName: classes.hideRightSeparator,
			renderCell: params => {
				const onUpdateClick = () => {
					history.push(`/author/list/${authorId}`);
				};
				const onDeleteClick = () => {
				    	checkToken(history,token);
						swal({
							title: "Supprimer l'auteur",
							text: 'Etes vous sur ?',
							icon: 'info',
							buttons: ['Non', 'Oui'],
							dangerMode: true,
				
						}).then(confirm => {
							if(confirm) {
								dispatch(deleteAuthor(authorId)).then(() => {
									swal({
										title: 'Auteur Supprimé!',
										icon: 'success'
									});
									dispatch(getAuthors())
								})
							}
						})
					
				}
				const onSendMailClick = e => {
					const formData = new FormData();
							formData.append("email",email);
							dispatch(sendMail(formData)).then(() => {
								
								swal({
									title: 'E-Mail de confirmation!',
									icon: 'success'
								});
								
							}).then(() => dispatch(getAuthors()))
				}
				return <UserRoleRow update={onUpdateClick} 
					remove={onDeleteClick}
					sendmail={onSendMailClick}
					confirm={authors.filter(author => author.id === params.row.id)[0].emailConfirmed}
				/>;
			}
		}
	];

	const rows = authors.map(author => ({
		...author,
		etat: 'validé',
		birthdate: moment(author.birthdate).format('YYYY-MM-DD'),
		inscriptionDate: '14-05-2010'
	}));

	const newRows = filterData(rows, searchQuery);

	useEffect(() => {
		dispatch(getAuthors());
		checkToken(history,token);
		
	}, []);

	useEffect(() => {
		dispatch(getCountries());
		if (isUpdated ==true) {
			dispatch(getAuthors());
		}

	}, [isUpdated]);
  if(loading) {
	return <Loading />
  }
	

	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center w-full">
					<div className="pt-10 pb-10">
						<div className="flex items-center">
							<Person className="text-32" />
							<span className="ml-8 text-16 md:text-24 font-semiblod">
								<b>Auteurs sans ME</b>
							</span>
						</div>
					</div>
					<div className='flex items-center'>
				   <Search
								onInput={e => {
									setSearchQuery(e.target.value)
								}}
								placeholder="rechercher par nom ou prénom"
							/>
					<Link
						to="/author/list/new"
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
					<ExportExcel excelData={ExcelExportData} fileName={"Authors"}/>
				</div>
				</div>
			}
			content={
				<div style={tableStyle}>
					<DataGrid
						onSelectionChange={newSelection => {
							setAuthorId(newSelection.rowIds[0]);
							const obj = authors.filter(
								author => author.id === newSelection.rowIds[0]
							)[0];
							setIsActive(!obj.isActive);
							setEmail(obj.email);
						}}
						pagination
						className={classes.root}
						rows={newRows}
						columns={columns}
						pageSize={10}
						rowsPerPageOptions={[5, 10, 20]}
						disableColumnMenu
						
						options={{
							draggable: false
						}}
					/>
				</div>
			}
		/>
	);
};

export default Authors;
