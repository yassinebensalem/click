import React, { useState, useEffect } from 'react';
import FusePageCarded from '@fuse/core/FusePageCarded';
import {
	Button,
	InputAdornment,
	TextField,
	Typography,
	FormControl,
	MenuItem,
	Select,
	InputLabel,
	Box,
} from '@material-ui/core';

import {
	ReceiptOutlined
} from '@material-ui/icons';

import { DataGrid } from '@material-ui/data-grid';
import { makeStyles, createStyles } from '@material-ui/styles';
import '../styles.css';
import moment from 'moment';
import { useSelector, useDispatch } from 'react-redux';
import { getEditors, findFirstEditorId } from 'app/store/editorSlice';

import SearchOrder from '../shared-components/SearchOrder';
import {getInvoicesByEditor} from 'app/store/billsSlice';
import ExportExcel from '../shared-components/ExcelExport';
import { useHistory } from 'react-router';
import { decode } from 'jsonwebtoken';

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
			position: 'relative',
			'& .MuiDataGrid-columnsContainer': {},
			'& .MuiDataGrid-colCellWrapper ': {
				borderTopLeftRadius: '20px',
				borderTopRightRadius: '20px'
			},
			'& .makeStyles-root-94': {
				background: '#fff'
			},
			borderTopLeftRadius: '20px',
			borderTopRightRadius: '20px'
		}
	})
);

const Bills = () => {
	const classes = useStyles();
	const dispatch = useDispatch();
	const { editors, firstEditorId } = useSelector(state => state.editor);
	const { bills } = useSelector(state => state.bill);
	const [searchQuery, setSearchQuery] = useState('');
	const {token} = useSelector(state => state.auth);
	const {history} = useHistory();


	const [selected, setSelected] = useState(false);



	const loadEditors = () => {
		dispatch(getEditors());
	};

	const reducer = (state, action) => {
		switch (action.type) {
		  case "FIND_FIRST_EDITOR":
			return {...state,
			arr: action.payload
		}
		  default:
			return state;
		}
	  };

	const [editorId, setEditorId] = useState('');  






	const handleChangeSelectEditor = e => {
		setEditorId(e.target.value);
		setSelected(true);
	};

	const getRaisonSocial = (publisherId) => {
		return editors.find(editor => editor.id === publisherId).raisonSocial;
	}


	const loadInvoices = (id) => {
		dispatch(getInvoicesByEditor(id));
	}
	useEffect(() => {
			loadEditors();
			
     if (decode(token).exp * 1000 < Date.now()) {
		localStorage.clear();
	     history.push('/')
       }
	}, [])
      
	useEffect(() => {
	    dispatch(findFirstEditorId(editors));
	
		if(selected==false) {
			loadInvoices(firstEditorId);
		}else {
			loadInvoices(editorId);
		}
	}, [editorId, editors, firstEditorId]);

	const ExcelExportData = bills.map(bill => ({
		'Titre':  bill.title,
		'Nom Du Client': `${bill.user.firstName} ${bill.user.lastName}`,
		'Maison d\'édition': getRaisonSocial(bill.book.publisherId),
		'Date De Facture': moment(bill.data).format('YYYY-MM-DD'),
		'Prix': bill.price,
		'Code Facture': bill.orderNumber

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
					<div
		
						className="rounded p-8"
					>
						<img
							src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${params.row.book.coverPath}`}
							alt="avatar"
							style={{
								width: '60px',
								height: '40px'
							}}
							className="w-full block rounded"
						/>
					</div>
				);
			}
		},
		{
			field: 'title',
			headerName: 'Titre',
			sortable: true,
			filter: false,
			headerClassName: classes.hideRightSeparator,
			flex: 1
		},
		{
			field: 'subscriberName',
			headerName: 'Nom du client',
			sortable: true,
			filter: false,
			headerClassName: classes.hideRightSeparator,
			flex: 1
		},
		{
			field: 'raisonSocial',
			headerName: "Maison d'édition",
			sortable: true,
			filter: false,
			headerClassName: classes.hideRightSeparator,
			flex: 1.5
		},

		{
			field: 'date',
			headerName: 'Date de facture',
			flex: 1,
			sortable: true,
			headerClassName: classes.hideRightSeparator
		},
		{
			field: 'price',
			headerName: 'Prix',
			sortable: true,
			headerClassName: classes.hideRightSeparator,
			flex: 1
		},
		{
			field: 'orderNumber',
			headerName: 'Code Facture',
			sortable: true,
			headerClassName: classes.hideRightSeparator,
			flex: 1
		}
	];

	const rows = bills.map(order => ({
		...order,
		'title': order.book.title,
		'subscriberName': `${order.user.firstName} ${order.user.lastName}`,
		'raisonSocial': getRaisonSocial(order.book.publisherId),
		'date': moment(order.date).format(
			'YYYY-MM-DD'
		),
		'price': order.price
	}));

    
	return (
		<FusePageCarded
			header={
				<div className="flex flex-col w-full">
					<div className="flex flex-1 justify-between items-center w-full">
						<div className="pt-10 pb-10">
							<div className="flex items-center">
								<div className="flex items-center">
									<ReceiptOutlined />
									<span className="ml-8 text-16 md:text-24 font-semiblod">
										<b>Factures</b>
									</span>
								</div>
							</div>
						</div>
						<div className="flex items-center">
							<SearchOrder
								onInput={e => {
									setSearchQuery(e.target.value);
								}}
								
							/>
						</div>
					</div>
					<div className="flex mb-10 items-center justify-between w-full">
						<Box
							sx={{
								minWidth: 400,
								marginLeft: '20px',
								marginTop: '5px',
								display: 'flex',
								justifyContent: 'space-between'
							}}
						>
							<FormControl fullWidth>
								<InputLabel
									id="demo-simple-select-label"
									style={{ color: '#fff', fontSize: '13px' }}
								>
									  {!selected ? editors[1]&& editors[1].raisonSocial: "choisisez une maison d'édition"}
								</InputLabel>
								<Select
									labelId="demo-simple-select-label"
									id="demo-simple-select"
									label={editors[0] && editors[0].raisonSocial}
									onChange={handleChangeSelectEditor}
								>
									{editors.map(editor => (
										<MenuItem
											value={editor.id}
										>{editor.raisonSocial}</MenuItem>
									))}
								</Select>
							</FormControl>
							
						</Box>
						<ExportExcel excelData={ExcelExportData} fileName={"Bills"}/>
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
					{rows.length > 0 ? (
						<DataGrid
							className={classes.root}
							pageSize={10}
							rowsPerPageOptions={[5, 10, 20]}
							rowHeight={100}
							columns={columns}
							
							disableColumnMenu
							sx={{
								'& .MuiDataGrid-cell:hover': {
									color: '#F00'
								}
							}}
						/>
					) : (
						<p
							style={{
								position: 'absolute',
								top: '50%',
								left: '50%',
								transform: 'translate(-50%,-50%)',
								fontFamily:
									'Inter var, Roboto, Helvetica, Arial, sans-serif',
								fontWeight: '400',
								fontSize: '2.4rem',
								lineHeight: '1.334',
								color: 'rgb(107, 114, 128)'
							}}
						>
							Il n'y pas de factures
						</p>
					)}
				</div>
			}
		/>
	);
};

export default Bills;
