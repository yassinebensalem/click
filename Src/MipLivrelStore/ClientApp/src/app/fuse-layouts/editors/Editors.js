import React, { useState, useEffect } from 'react';
import FuseCountdown from '@fuse/core/FuseCountdown';
import FusePageCarded from '@fuse/core/FusePageCarded';
import book from '../../../images/book.png';
import {
	Box,
	Button,
	CircularProgress,
	InputAdornment,
	TextField
} from '@material-ui/core';
import {
	DeleteOutline,
	Edit,
	Settings,
	AddOutlined,
	Check,
	ShoppingBasketOutlined,
	Add,
	Person
} from '@material-ui/icons';
import { Link } from 'react-router-dom';
import { DataGrid } from '@material-ui/data-grid';
import { makeStyles, createStyles } from '@material-ui/styles';
import { deleteEditor, getEditors, sendMail } from 'app/store/editorSlice';
import '../styles.css';
import moment from 'moment';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import Loading from '../shared-components/Loading';
import swal from 'sweetalert';
import UserRoleRow from '../shared-components/UserRoleRow';
import SwitchRow from '../shared-components/SwitchRowEditor';
import Search from '../shared-components/Search';
import { selectMainThemeLight } from 'app/store/fuse/settingsSlice';
import ExportExcel from '../shared-components/ExcelExport';
import SwitchRowEditor from '../shared-components/SwitchRowEditor';
import { decode } from 'jsonwebtoken';
import { getCountries } from 'app/store/countrySlice';

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

const Editors = () => {
	const history = useHistory();
	const classes = useStyles();
	const [status, setStatus] = useState(false);
	const [editorId, setEditorId] = useState('');
	const [email, setEmail] = useState('');
	const [editor, setEditor] = useState('');
	const { editors, loading, isUpdated } = useSelector(state => state.editor);
	const [searchQuery, setSearchQuery] = useState('');
	const {token} = useSelector(state => state.auth);
	const dispatch = useDispatch();
	const {countries} = useSelector(state => state.country);
	const handleClick = () => {
		setEditor(editors.filter(editor => editor.id === editorId)[0]);
	};
	const [isActive, setIsActive] = useState(false);
	useEffect(() => {
		if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
		}
		dispatch(getEditors());
		dispatch(getCountries())
	}, []);


	const filterData = (data, query) => {
		if (!query) {
			return data;
		} else {
			return data.filter(
				d =>
					(d.raisonSocial || '')
						.toLowerCase()
						.includes((query || '').toLowerCase()) ||
					d.idFiscal
						.toLowerCase()
						.includes((query || '').toLowerCase())
			);
		}}
	const newRows = filterData(editors, searchQuery);
	const rows = newRows.map((row) => ({
		...row,
		inscriptionDate: moment(row.createdAt).format('YYYY-MM-DD')
	}))
	const getCountryName = (id) => {
		const obj = countries.filter(country => country.id === id)[0];
		if (obj !== undefined) {
			return obj.name;
		}
	};
	const ExcelExportData = editors.map(editor => ({
		'Raison Social': editor.raisonSocial,
		'idFiscal': editor.idFiscal,
		'phoneNumber': editor.phoneNumber,
		'firstName': editor.firstName,
		'lastName': editor.lastName,
		'email':editor.email,
		'address': editor.address,
		'countryName': getCountryName(editor.countryId),
		'idFiscal': editor.idFiscal,
		'inscriptionDate': '14-05-2010'
	}))

	useEffect(() => {
		if (isUpdated) {
			dispatch(getEditors());
		}
	}, [isUpdated]);
  


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
			field: 'raisonSocial',
			headerName: 'Raison Social',
			headerClassName: classes.hideRightSeparator,
			flex: 1,
			editable: true,
			headerAlign: 'center',
			align: 'center'
		},
		{
			field: 'idFiscal',
			headerName: 'Id Fiscal',

			headerClassName: classes.hideRightSeparator,
			width: 150,
			editable: true,
			headerAlign: 'center',
			align: 'center'
		},
		{
			field: 'inscriptionDate',
			headerName: 'Date D\'inscription',
			width: 160,
			headerClassName: classes.hideRightSeparator,
			headerAlign: 'center',
			align: 'center',

		},
		{
			field: 'phoneNumber',
			headerName: 'Numéro de téléphone',
			flex: 1,
			headerClassName: classes.hideRightSeparator,
		
			editable: true,
			headerAlign: 'center',
			align: 'center',
		},
		{
			field: 'Status',
			headerName: 'Etat Du Compte',
			width: 160,
			headerClassName: classes.hideRightSeparator,

			renderCell: params => {
				return (
					<SwitchRowEditor
						handleClick={handleClick}
						editorId={params.row.id}
						editor={
							editors.filter(
								editor => editor.id === params.row.id
							)[0]
						}
						isActive={
							editors.filter(
								editor => editor.id === params.row.id
							)[0].isActive
						}
					/>
				);
			}
		},

		{
			field: 'actions',
			headerName: 'Actions',
			align:'center',
			headerAlign:'center',
			headerClassName: classes.hideRightSeparator,
			renderCell: params => {
				const onUpdateClick = e => {
					history.push(`/editor/list/${editorId}`);
				};
				const onDeleteClick = e => {
	
					swal({
						title: "Supprimer l'editeur",
						text: 'Etes vous sur ?',
						icon: 'info',
						buttons: ['Non', 'Oui'],
						dangerMode: true,
			
					}).then(confirm => {
						if(confirm) {
							dispatch(deleteEditor(editorId)).then(() => {
								swal({
									title: 'Editeur Supprimé!',
									icon: 'success'
								});
								dispatch(getEditors())
							})
						}
					})
				
		    	}
				const onSendMailClick = e => {
					const formData = new FormData();
							formData.append("email",email);
							dispatch(sendMail(formData)).then(() => {
								swal({
									title: 'E-Mail de confirmation reçu!',
									icon: 'success'
								});
							})
				}

				return <UserRoleRow update={onUpdateClick}
				sendmail={onSendMailClick}
				remove={onDeleteClick}
				confirm={editors.filter(editor => editor.id === params.row.id)[0].emailConfirmed}
				 />;
			}
		}
	];
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
								<b>Editeurs</b>
							</span>
						</div>
					</div>
                   <div className='flex items-center'>
				   <Search
								onInput={e => {
									setSearchQuery(e.target.value)
								}}
								placeholder="rechercher par raison social ou id fiscal"
							/>
					<Link
						to="/editor/list/new"
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
					<ExportExcel excelData={ExcelExportData} fileName={"Editors"}/>
					
					</div>
				</div>
			}
			content={
				<div
					style={{
						borderTopLeftRadius: '20px',
						borderTopRightRadius: '20px',
						width: '100%',
						height: '100%'
					}}
				>
					<DataGrid
						onSelectionChange={newSelection => {
							setEditorId(newSelection.rowIds[0]);
							const obj = editors.filter(
								editor => editor.id === newSelection.rowIds[0]
							)[0];
							setIsActive(obj.isActive);
							setEmail(obj.email);
						}}
						className={classes.root}
						rows={rows}
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

export default Editors;
