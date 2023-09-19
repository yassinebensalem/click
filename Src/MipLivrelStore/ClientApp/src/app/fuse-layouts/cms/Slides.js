import FusePageCarded from '@fuse/core/FusePageCarded';
import { Button, Checkbox, FormControlLabel, FormGroup } from '@material-ui/core';
import { Link } from 'react-router-dom';
import React, { useCallback, useState } from 'react';
import { DataGrid } from '@material-ui/data-grid';
import { DeleteOutline, Edit, Settings, PersonOutline, ShoppingBasketOutlined, Adb } from '@material-ui/icons';
import { makeStyles, createStyles } from '@material-ui/styles';
import { AgGridReact, AgGridColumn } from 'ag-grid-react';
import 'ag-grid-community/dist/styles/ag-grid.css';
import 'ag-grid-community/dist/styles/ag-theme-alpine.css';
import '../styles.css';
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
			borderRadius: '20px'
		}
	})
);
const Slides = () => {
	const classes = useStyles();
	const label = { inputProps: { 'aria-label': 'Checkbox demo' } };
	const [checked, setChecked] = useState(false);
	const addDropZones = params => {
		var tileContainer = document.querySelector('.tile-container');
		var dropZone = {
			getContainer: function () {
				return tileContainer;
			},
			onDragStop: function (params) {
				var tile = createTile(params.node.data);
				tileContainer.appendChild(tile);
			}
		};
		params.api.addRowDropZone(dropZone);
	};
	const createTile = data => {
		var el = document.createElement('div');
		el.classList.add('tile');
		el.classList.add(data.color.toLowerCase());
		el.innerHTML =
			'<div class="id">' +
			data.id +
			'</div>' +
			'<div class="value">' +
			data.value1 +
			'</div>' +
			'<div class="value">' +
			data.value2 +
			'</div>';
		return el;
	};

	const columns = [
		{
			field: 'id',
			headerName: 'ID',
			cellStyle: { 'padding-left': '25px' },
			headerCheckBoxSelection: true,
			flex: 1
		},

		/*{
				field: '',
				headerName: '',
				headerAlign: 'center',
				sortable: false,
				width: 80,
				headerClassName: classes.hideRightSeparator,
	
				renderCell: params => {
					return (
						<div
							style={{
								background: '#ddd'
							}}
							className="rounded p-3"
						>
							<img
								src={params.row.image}
								alt="avatar"
								style={{
									maxWidth: '100%',
									height: '100%'
								}}
								className="w-full block rounded"
							/>
						</div>
					);
				}
			}*/ {
			field: 'Title',
			headerName: 'Titre',
			cellStyle: { 'padding-left': '25px' },
			flex: 1
		},
		{
			field: 'SubTitle',
			headerName: 'Sous Titre',
			cellStyle: { 'padding-left': '25px' },
			flex: 1
		},
		{
			field: 'Link',
			headerName: 'Lien',
			cellStyle: { 'padding-left': '25px' },
			flex: 1
		},
		{
			field: 'actions',
			headerName: 'Actions',
			cellStyle: { 'padding-left': '25px' },
			flex: 1,
			cellRenderer: params => {
				const onDeleteClick = e => {
					e.stopPropagation();
				};
				const onUpdateClick = e => {
					e.stopPropagation();
				};
				return (
					<div>
						<DeleteOutline
							onClick={onDeleteClick}
							className="cursor-pointer"
							titleAccess="delete"
							style={{ color: '#8D8F8F' }}
						/>
						<Edit
							onClick={onUpdateClick}
							className="cursor-pointer"
							titleAccess="edit"
							style={{ color: '#8D8F8F' }}
						/>
					</div>
				);
			}
		}
	];

	const rows = [
		{
			id: 1,
			Title: 'Publicite 1',
			SubTitle: 'Description de Publicite 1',
			Link: 'http://www.mylink.com'
		},
		{
			id: 2,
			Title: 'Publicite 2',
			SubTitle: 'Description de Publicite 2',
			Link: 'http://www.mylink.com'
		},
		{
			id: 3,
			Title: 'Publicite 3',
			SubTitle: 'Description de Publicite 3',
			Link: 'http://www.mylink.com'
		}
	];
	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center w-full">
					<div className="pt-10 pb-10">
						<div className="flex items-center">
							<Adb
								style={{
									fontSize: '32px'
								}}
							/>
							<span className="ml-8 text-16 md:text-24 font-semiblod">
								<b>Slides</b>
							</span>
						</div>
					</div>
					<Link to="/cms/slides/new" style={{ color: '#fff', textDecoration: 'none' }}>
						<Button
							style={{
								backgroundColor: '#88CF00',
								color: '#fff'
							}}
							size="small"
							variant="container"
							className="mx-2 h-28 rounded-2xl pt-20 pb-20"
							color="primary"
						>
							Ajouter un Slide
						</Button>
					</Link>
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
					className="ag-theme-balham"
				>
					{/*<DataGrid
						className={classes.root}
						disableSelectionOnClick
						rows={rows}
						columns={columns}
						pageSize={5}
						rowsPerPageOptions={[10]}
						checkboxSelection
						disableColumnMenu
					/>*/}

					<AgGridReact
						columnDefs={columns}
						rowData={rows}
						rowDragManaged={true}
						rowDragEntireRow={true}
						allowDragFromColumnsToolPanel={false}
						animateRows={true}
						gridOptions={{
							headerHeight: 50,
							rowHeight: 80,
							rowStyle: {
								padding: 50
							}
						}}
					>
						<AgGridColumn field="id"></AgGridColumn>
						<AgGridColumn field="Title"></AgGridColumn>
						<AgGridColumn field="SubTitle"></AgGridColumn>
						<AgGridColumn field="Link"></AgGridColumn>
					</AgGridReact>
				</div>
			}
		/>
	);
};

export default Slides;
