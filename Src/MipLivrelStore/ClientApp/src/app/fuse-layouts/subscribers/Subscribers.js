import FuseCountdown from '@fuse/core/FuseCountdown';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { Button } from '@material-ui/core';
import { DataGrid } from '@material-ui/data-grid';
import {
	Add,
	Person
} from '@material-ui/icons';
import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { makeStyles, createStyles } from '@material-ui/styles';
import { useSelector, useDispatch } from 'react-redux';
import { deleteSubscriber, getSubscribers } from 'app/store/subscriberSlice';
import '../styles.css';
import UserRoleRow from '../shared-components/UserRoleRow';
import moment from 'moment';
import { useHistory } from 'react-router';
import SwitchRow from '../shared-components/SwitchRowSubscriber';
import swal from 'sweetalert';
import SubscriberRow from '../shared-components/SubscriberRow';
import ExportExcel from '../shared-components/ExcelExport';
import { decode } from 'jsonwebtoken';
import Loading from '../shared-components/Loading';
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
const Subscribers = () => {
	const classes = useStyles();
	const dispatch = useDispatch();
	const { subscribers, isUpdated, loading } = useSelector(state => state.subscriber);
	const [subscriberId, setSubscriberId] = useState('');
	const [subscriber, setSubscriber] = useState('');
	const history = useHistory();
	const {token} = useSelector(state => state.auth);
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
			width: 130,
			headerClassName: classes.hideRightSeparator,
			flex: 1,
			headerAlign:'center',
			align:'center',
		},
		{
			field: 'lastName',
			headerName: 'Nom',
			width: 150,
			headerClassName: classes.hideRightSeparator,
			flex: 1,
			headerAlign:'center',
			align:'center',
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
			field: 'email',
			headerName: 'E-mail',
			width: 170,
			headerClassName: classes.hideRightSeparator,
			headerAlign:'center',
			align:'center',
			flex: 2
		},
		{
			field: 'Status',
			headerName: 'Etat Du Compte',
			width: 170,

			flex: 2,
			renderCell: params => {
				return <SwitchRow
					handleClick={handleClick}
					subscriberId={params.row.id}
					subscriber={
						subscribers.filter(subscriber => subscriber.id === params.row.id)[0]
					}
					isActive={subscribers.filter(subscriber => subscriber.id === params.row.id)[0].isActive}
				/>;
			}
		},
		{
			field: 'actions',
			headerName: 'Actions',
			width: 170,
			headerClassName: classes.hideRightSeparator,
			flex: 1,
			renderCell: params => {
				const onUpdateClick = e => {
					history.push(`/subscriber/list/${subscriberId}`);
				};
				const onDeleteClick = e => {
				
					swal({
						title: "Supprimer le client",
						text: 'Etes vous sur ?',
						icon: 'info',
						buttons: ['Non', 'Oui'],
						dangerMode: true,
			
					}).then(confirm => {
						if(confirm) {
							dispatch(deleteSubscriber(subscriberId)).then(() => {
								swal({
									title: 'Client Supprimé!',
									icon: 'success'
								});
								dispatch(getSubscribers())
							})
						}
					})
				
			}
				return <SubscriberRow update={onUpdateClick}
				remove={onDeleteClick}
				 />;
			}
		}
	];
	const loadSubscribers = () => {
		dispatch(getSubscribers());
	};
	useEffect(() => {
		if (decode(token).exp * 1000 < Date.now()) {
			history.push('/')
		}
		loadSubscribers();
	}, []);


	useEffect(() => {
	
		if(isUpdated) {
			loadSubscribers();
		}
	
	}, [isUpdated]);
	const handleClick = () => {
		setSubscriber(subscribers.filter(subscriber => subscriber.id === subscriberId)[0]);
	};
	const rows = subscribers.map(subscriber => ({
		...subscriber,
		birthdate: moment(subscriber.birthdate).format('DD-MM-YYYY'),
		inscriptionDate: (subscriber.createdAt !== null) ? moment(subscriber.createdAt).format('YYYY-MM-DD') : '14-05-2010'
	}));
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
								<b>Clients</b>
							</span>
	
						</div>
					
					</div>
					<ExportExcel excelData={subscribers} fileName={"Subscribers"}/>
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
						className={classes.root}
						rows={rows}
						columns={columns}
						pageSize={10}
				
						rowsPerPageOptions={[5, 10, 20]}
						disableColumnMenu
						onSelectionChange={newSelection => {
							setSubscriberId(newSelection.rowIds[0]);
						}}
					/>
				</div>
			}
		/>
	);
};

export default Subscribers;
