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
	IconButton,
	Paper
} from '@material-ui/core';

import {
	DeleteOutline,
	LibraryBooks,
	UpdateOutlined,
	Edit,
	Delete,
	EditOutlined,
	Settings,
	MoreHoriz,
	DeleteOutlined,
	FilterList,
	FindReplace,
	Add,
	MenuBook
} from '@material-ui/icons';

import { Link, useHistory } from 'react-router-dom';
import { DataGrid } from '@material-ui/data-grid';
import { makeStyles, createStyles } from '@material-ui/styles';
import '../styles.css';
import moment from 'moment';
import { useSelector, useDispatch } from 'react-redux';
import { getBooks, filterBooks, updateBookState, deleteBook } from 'app/store/bookSlice';
import { getCategories } from 'app/store/categorySlice';
import { getLanguages } from 'app/store/languageSlice';
import { getAuthorsWithoutAccount } from 'app/store/authorWithoutAccountSlice';
import Loading from '../shared-components/Loading';
import { KeyboardDatePicker } from '@material-ui/pickers';
import { getEditors } from 'app/store/editorSlice';
import Search from '../shared-components/Search';
import BookRow from '../shared-components/BookRow';
import swal from 'sweetalert';

import styled from 'styled-components';
import ExportExcel from '../shared-components/ExcelExport';
import { decode } from 'jsonwebtoken';
import { checkToken } from 'app/services/Helpers';

const useStyles = makeStyles(theme =>
	createStyles({
		hideRightSeparator: {
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
const NoBooksStyle = {
	position: 'absolute',
	top: '50%',
	left: '50%',
	transform: 'translate(-50%,-50%)',
	fontFamily: 'Inter var, Roboto, Helvetica, Arial, sans-serif',
	fontWeight: '400',
	fontSize: '2.4rem',
	lineHeight: '1.334',
	color: 'rgb(107, 114, 128)'
};
const BooksList = () => {
	const classes = useStyles();
	const [title, setTitle] = useState('');
	const [categoryId, setCategoryId] = useState('');
	const [editorId, setEditorId] = useState('');
	const dispatch = useDispatch();
	const { books, loading } = useSelector(state => state.book);
	const { categories } = useSelector(state => state.category);
	const { languages } = useSelector(state => state.language);
	const { authorsWithoutAccount } = useSelector(state => state.authorWithoutAccount);
	const [selectedStartDate, setSelectedStartDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	const [selectedEndDate, setSelectedEndDate] = useState(
		moment(new Date()).format('YYYY-MM-DD')
	);
	const { editors } = useSelector(state => state.editor);

	const [searchQuery, setSearchQuery] = useState('');
	const [value, setValue] = useState('');
	const history = useHistory();
	const [statusId, setStatusId] = useState('');
	const booksFilteredByStatus = id => {
		return books.filter(book => book.status === id);
	};
	const {token} = useSelector(state => state.auth);
	
	const filterData = (data, query) => {
		if (!query) {
			return data;
		} else {
			return data.filter(
				d =>
					(d.title || '')
						.toLowerCase()
						.includes((query || '').toLowerCase()) ||
					d.categoryName
						.toLowerCase()
						.includes((query || '').toLowerCase())
			);
		}
	};

	const getCategoryName = id => {
		const category = categories.filter(category => category.id === id)[0];

		return category && JSON.parse(category.categoryName).fr;
	};
	const getLanguageName = id => {
		const language = languages.filter(language => language.id === id)[0];
		return language && language.name;
	};

	const getRaisonSocial = id => {
		const publisher = editors.find(editor => editor.id === id);
		return publisher && publisher.raisonSocial;
	}
    
	const getAuthorName = id =>  {
		const author = authorsWithoutAccount.filter(author => author.id === id)[0];
		return author && `${author.firstName} ${author.lastName}`;
	}


	
	const rowsFilteredByStatus = booksFilteredByStatus(statusId);
	const rows = statusId
		? rowsFilteredByStatus.map(book => ({
				...book,
				publicationDate: moment(book.publicationDate).format(
					'YYYY-MM-DD'
				),
				createdAt: moment(book.createdAt).format(
					'YYYY-MM-DD'
				),
				authorName: getAuthorName(book.authorId),
				categoryName: getCategoryName(book.categoryId),
				Language: getLanguageName(book.languageId),
				raisonSocial: getRaisonSocial(book.publisherId),
				Status: statusId,
				updatedAt: moment(book.updatedAt).format(
					'YYYY-MM-DD'
				)
		  }))
		: books.map(book => ({
				...book,
				publicationDate: moment(book.publicationDate).format(
					'YYYY-MM-DD'
				),
				createdAt: moment(book.createdAt).format(
					'YYYY-MM-DD'
				),
				authorName: getAuthorName(book.authorId),
				categoryName: getCategoryName(book.categoryId),
				Language: getLanguageName(book.languageId),
				raisonSocial: getRaisonSocial(book.publisherId),
				Status: book.status,
				updatedAt: moment(book.updatedAt).format(
					'YYYY-MM-DD'
				)
		  }));

	const loadBooks = async () => {
		dispatch(getBooks());
	};
	const loadCategories = () => {
		dispatch(getCategories());
	};

	const loadLanguages = () => {
		dispatch(getLanguages());
	};

	const loadEditors = () => {
		dispatch(getEditors());
	};

	const loadAuthorsWithoutAccount = () => {
		dispatch(getAuthorsWithoutAccount());
	}

	useEffect(() => {
		loadBooks();
		loadCategories();
		loadLanguages();
		loadEditors();
		loadAuthorsWithoutAccount();
		checkToken(history,token);
	}, []);

	
	const handleChangeSelectEditor = e => {
		setEditorId(e.target.value);
	};
	const handleChangeSelectStatus = e => {
		setStatusId(e.target.value);
	};

	const onChangeStartDate = (e, row) => {
		setSelectedStartDate(row);
	};
	const onChangeEndDate = (e, row) => {
		setSelectedEndDate(row);
	};

	const newRows = filterData(rows, searchQuery);

	const filter = () => {
		const obj = {
			FromDate: selectedStartDate,
			ToDate: selectedEndDate,
			publisherId: editorId
		};
		dispatch(filterBooks(obj));
	};
	

	const ExcelExportData = books.map(book => ({
		'Titre': book.title,
		'Maison d`\édition': getRaisonSocial(book.publisherId),
		'Nom d\'auteur': getAuthorName(book.authorId),
		'Catégorie': getCategoryName(book.categoryId),
		'Langue': getLanguageName(book.languageId),
		'Prix': book.price,
		'Date de création': moment(book.createdAt).format(
			'YYYY-MM-DD'
		),
		"Description": book.description,
		"Nombre de pages": book.pageNumbers,
		"ISBN": book.isbn

	}))

	const columns = [
		{
			field: '',
			headerName: '',
			headerAlign: 'center',
			sortable: false,
			width: 80,
			renderCell: params => {
				console.log(params);
				return (
					<div
						style={{
							backgroundColor: '#eee'
						}}
						className="rounded p-2"
					>
						<img
							src={`https://miplivrelstorage.blob.core.windows.net/bookscover/${params.row.coverPath}`}
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
			field: 'title',
			headerName: 'Titre',
			filter: false,
			align: 'center',
			headerAlign: 'center',
			width: 200,
			renderCell: (cellValue) =>  {
				return (
					<div style={{ width:'100%', display:'flex', alignItems:'center',
					}}>
               <TextField 
					value={cellValue.value}
					InputProps={{ disableUnderline: true, style: { textAlign:"center", padding:"5px"}}}
					multiline={true}
		

				/>
				</div>
				)
				
			}
		},
		{
			field: 'authorName',
			headerName: 'Nom d\'auteur',
			filter: false,
			width: 130,
			align:'center',
			headerAlign:'center',
			renderCell: (cellValue) => {
				return (
					<div
					style={{ width:'100%', display:'flex', alignItems:'center',
					}}
					>
						<TextField 
						  value={cellValue.value}
                          multiline={true}
						  InputProps={{disableUnderline: true}}
						/>
					</div>
				)
			}
		},
		{
			field: 'raisonSocial',
			headerName: 'Maison d\'édition',
			filter: false,
			width: 230,
			align: 'center',
			headerAlign: 'center'
		},
		{
			field: 'categoryName',
			headerName: 'Catégorie',
			width: 120,
			align: 'center',
			headerAlign: 'center'
		},

		{
			field: 'Language',
			headerName: 'Langue',
			minWidth: 120,
			align: 'center',
			headerAlign: 'center'
		},
		{
			field: 'price',
			headerName: 'Prix',
			minWidth: 120,
			align: 'center',
			headerAlign: 'center',
			renderCell: params => {
               return (
				 <div
				 style={{
					display:'flex',
					alignItems:'center',
					justifyContent:'center',
					width:'100%'
				 }}
				 >
                   {Math.ceil(params.row.price)}
				 </div>
			   )
			}
		},
		{
			field: 'createdAt',
			headerName: 'Date Créa',
			align: 'center',
			headerAlign: 'center',
			width: 120
		},
		{
			field: 'updatedAt',
			headerName: 'Date Modif',
			align: 'center',
			headerAlign:'center',
			width: 120,
			renderCell: params => {
					return (
						<div
						style={{
								display:'flex',
								width:'100%',
								alignItems:'center',
								justifyContent:'center'
								
							}}
						>
							<span>
								{moment(params.row.updatedAt).format(
									'YYYY-MM-DD'
								)}
							</span>
						</div>
					);}
	
		},
		{
			field: 'Status',
			headerName: 'Status',
			align: 'center',
			headerAlign: 'center',
			flex: 1,
			renderCell: params => {
				if (params.value === 1) {
					return (
						<div className="bg-purple-700 text-white  py-2 px-8 rounded-full center h-28">
							Crée
						</div>
					);
				} else if (params.value === 2) {
					return (
						<div className="bg-red-700 text-white  py-4 px-12 rounded-full center h-28">
							Rejeté
						</div>
					);
				}
				if (params.value == 3) {
					return (
						<div className="bg-green-700 text-white  py-2 px-12 rounded-full center h-28">
							Publié
						</div>
					);
				} else {
					return (
						<div className="bg-blue-700 text-white  py-4 px-5 rounded-full center h-28 w-500">
							Non publié
						</div>
					);
				}
			}
		},
		{
			field: 'actions',
			headerName: 'Actions',
			sortable: true,
			headerClassName: classes.hideRightSeparator,
			width: 180,
			align: 'left',
			headerAlign: 'center',
			renderCell: params => {
				const onDeleteClick = e => {
					checkToken(history,token);
					e.stopPropagation();
					swal({
						title: `Refuser ${params.row.title}`,
						text: 'Etes vous sur ?',
						icon: 'error',
						buttons: ['Non', 'Oui'],
						dangerMode: true
					}).then(() => {
						dispatch(
							updateBookState({ Id: params.row.id, Status: 2 })
						).then(originalPromiseResult => {
							swal({
								title: 'Livre Rejeté!',
								icon: 'success'
							});
							dispatch(getBooks());
						});
					});
				};

				const onRemoveClick = e => {
				
					swal({
						title: "Supprimer le livre",
						text: 'Etes vous sur ?',
						icon: 'info',
						buttons: ['Non', 'Oui'],
						dangerMode: true,
					}).then(confirm => {
						if(confirm) {
							dispatch(deleteBook(params.row.id)).then(() => {
								swal({
									title: 'Livre Supprimé!',
									icon: 'success'
								});
								dispatch(getBooks())
							})
						}
					})
				
			}

				const onValidateClick = e => {
					e.stopPropagation();
					swal({
						title: `Publier ${params.row.title}`,
						text: 'Etes vous sur ?',
						icon: 'success',
						buttons: ['Non', 'Oui'],
						dangerMode: true
					}).then(() => {
						dispatch(
							updateBookState({ Id: params.row.id, Status: 3 })
						).then(originalPromiseResult => {
							dispatch(getBooks());
						});
					});
				};
				const onEditClick = e => {
					e.stopPropagation();
					history.push(`/book/list/${params.row.id}`);
				};

				const onNotPublish = e => {
					e.stopPropagation();
					swal({
						title: `Ne pas publier ${params.row.title}`,
						text: 'Etes vous sur ?',
						icon: 'success',
						buttons: ['Non', 'Oui'],
						dangerMode: true
					}).then(() => {
						dispatch(
							updateBookState({ Id: params.row.id, Status: 4 })
						).then(originalPromiseResult => {
							dispatch(getBooks());
						});
					});
				};
				
				return (
					<BookRow
						validate={onValidateClick}
						cancel={onDeleteClick}
						rejected={false}
						edit={onEditClick}
						status={params.row.status}
						notPublish={onNotPublish}
						remove={onRemoveClick}
					/>
				);
			}
		}
	];

	if (loading) {
		return <Loading />;
	}

	return (
		<FusePageCarded
			header={
				<div className="flex flex-col w-full">
					<div className="flex flex-1 justify-between items-center w-full">
						<div className="pt-10 pb-10">
							<div className="flex items-center">
								<div className="flex items-center">
									<MenuBook />
									<span className="ml-8 text-16 md:text-24 font-semiblod">
										<b>Livres</b>
									</span>
								</div>
							</div>
						</div>
						<div className="flex items-center">
							<Search
								onInput={e => {
									setSearchQuery(e.target.value);
								}}
								placeholder="Rechercher un livre (titre, catégorie)"
							/>
							<Link
								to="/book/new"
								style={{ textDecoration: 'none' }}
							>
								<Button
									className="mx-2   
								h-28 
								rounded-2xl 
								pt-20 
								pb-20 
								muiltr-ho483n  
								flex 
								items-center 
								space-between 
								ml-8"
									style={{
										backgroundColor: '#f75454',
										color: '#fff',
										fontWeight: '500',
										minHeight: '20px'
									}}
									size="small"
									variant="container"
									color="secondary"
									onClick={filter}
								>
									<Add className="mr-8" />
									Ajouter
								</Button>
							</Link>
							<ExportExcel excelData={ExcelExportData} fileName={"Books"}/>
						</div>
					</div>
					<div className="flex mb-10 items-center space-between">
						<Box
							sx={{
								width: '20%',
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
									Choisir l'editeur
								</InputLabel>
								<Select
									labelId="demo-simple-select-label"
									id="demo-simple-select"
									label="Choisir  l'editeur"
									onChange={handleChangeSelectEditor}
								>
									{editors.map(editor => (
										<MenuItem value={editor.id}>
											{editor.raisonSocial}
										</MenuItem>
									))}
								</Select>
							</FormControl>
						</Box>
						<Box
							sx={{
								width: '20%',
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
									Choisir le status du livre
								</InputLabel>
								<Select
									labelId="demo-simple-select-label"
									id="demo-simple-select"
									label="Choisir  le Status"
									onChange={handleChangeSelectStatus}
								>
									<MenuItem value={1}>Crée</MenuItem>
									<MenuItem value={2}>Rejeté</MenuItem>
									<MenuItem value={3}>Publié</MenuItem>
									<MenuItem value={4}>Non Publié</MenuItem>
								</Select>
							</FormControl>
						</Box>

						<KeyboardDatePicker
							autoOk
							variant="inline"
							inputVariant="outlined"
							label="Date de début"
							value={selectedStartDate}
							format={'YYYY-MM-DD'}
							InputAdornmentProps={{ position: 'start' }}
							onChange={onChangeStartDate}
							className="ml-10 mt-5 mb-8"
						/>
						<span className="ml-7">à</span>
						<KeyboardDatePicker
							autoOk
							variant="inline"
							inputVariant="outlined"
							label="Date de fin"
							value={selectedEndDate}
							format={'YYYY-MM-DD'}
							InputAdornmentProps={{ position: 'start' }}
							onChange={onChangeEndDate}
							className="ml-10 mt-5 mb-8"
						/>

						<Button
							style={{
								backgroundColor: '#f75454',
								marginLeft: '5px'
							}}
							onClick={filter}
						>
							<FindReplace />
						</Button>
					</div>
				</div>
			}
			content={
				<div
					style={{
						borderTopLeftRadius: '20px',
						borderTopRightRadius: '20px',
						width: '100%',
						height: '100%',
						fontSize: '20px'
					}}
				>
					{newRows.length > 0 ? (
						<DataGrid
							className={classes.root}
							rows={newRows}
							rowHeight={70}
							columns={columns}
							pageSize={10}
							rowsPerPageOptions={[5, 10, 20]}
							disableColumnMenu
							sx={{
								'& .MuiDataGrid-cell:hover': {
									color: '#F00'
								}
							}}
						/>
					) : (
						<p style={NoBooksStyle}>Il n'y pas de livres!</p>
					)}
				</div>
			}
		/>
	);
};

export default BooksList;
