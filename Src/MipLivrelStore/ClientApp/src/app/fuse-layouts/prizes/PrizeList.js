import React, { useEffect } from 'react';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { DataGrid } from '@material-ui/data-grid';
import { makeStyles, createStyles } from '@material-ui/styles';
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
	LibraryBooks,
	Add
} from '@material-ui/icons';
import EmojyEvents from '@material-ui/icons/EmojiEvents';
import { Link } from 'react-router-dom';
import swal from 'sweetalert';
import PrizeRow from '../shared-components/PrizeRow';
import { getPrizes } from 'app/store/prizeSlice';
import { getCountries } from 'app/store/countrySlice';
import { useDispatch, useSelector } from 'react-redux';
import Loading from '../shared-components/Loading';
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
const PrizeList = () => {
	const classes = useStyles();
	const { prizes, loading } = useSelector(state => state.prize);
	const { countries } = useSelector(state => state.country);
	const dispatch = useDispatch();
	const newRows = [
		{
			id: 1,
			title: 'Prize 1',
			country: 'Germany'
		},
		{
			id: 2,
			title: 'Prize 2',
			country: 'Algeria'
		}
	];
	const loadPrizes = () => {
		dispatch(getPrizes());
	};
	const loadCountries = () => {
		dispatch(getCountries());
	};

	useEffect(() => {
		loadPrizes();
		loadCountries();
	}, []);
	const getCountryName = id => {
		const country = countries.filter(country => country.id === id)[0];
		return country && country.name;
	};

	const rows = prizes.map(prize => ({
		...prize,
		country: getCountryName(prize.countryId)
	}));
	const columns = [
		{
			field: 'name',
			headerName: 'Titre',
			width: 150,
			headerClassName: classes.hideRightSeparator,
			flex: 2,
			editable: true
		},
		{
			field: 'country',
			headerName: 'Pays',
			width: 130,
			headerClassName: classes.hideRightSeparator,
			flex: 3,
			editable: true
		},
		{
			field: 'actions',
			headerName: 'Actions',

			flex: 1,
			headerClassName: classes.hideRightSeparator,
			renderCell: params => {
				const onDeleteClick = e => {
					e.stopPropagation();
					swal({
						text: 'Voulez vous supprimer cet éditeur ?',
						icon: 'warning',
						buttons: ['Non', 'Oui'],
						dangerMode: true
					});
				};

				return <PrizeRow />;
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
									<EmojyEvents
										style={{
											fontSize: '40px'
										}}
									/>
									<span className="ml-8 text-16 md:text-24 font-semiblod">
										<b>Prix</b>
									</span>
								</div>
							</div>
						</div>
						<div className="flex items-center">
							<Link
								to="/bookPrize/new"
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
												ml-8
												save-btn
												"	
									size="small"
									variant="container"
									color="secondary"
								>
									<Add className="mr-8" />
									Ajouter
								</Button>
							</Link>
						</div>
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
					{newRows.length > 0 ? (
						<DataGrid
							className={classes.root}
							rows={rows}
							rowHeight={100}
							columns={columns}
							pageSize={5}
							disableColumnMenu
							options={{
								draggable: false
							}}
							sx={{
								border: 0, // also tried setting to none
								borderRadius: 2,
								p: 2,
								minWidth: 300,
								backgroundColor: '#ff0'
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
							Il n'y pas de prix!
						</p>
					)}
				</div>
			}
		/>
	);
};

export default PrizeList;
