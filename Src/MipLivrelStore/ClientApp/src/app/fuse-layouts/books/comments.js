import React, { useState } from 'react';
import FuseCountdown from '@fuse/core/FuseCountdown';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { Comment } from '@material-ui/icons';
import { DataGrid } from '@material-ui/data-grid';
import { Link } from 'react-router-dom';
import { MenuItem, FormControl, Box, Select, OutlinedInput, InputLabel } from '@material-ui/core';
const Comments = () => {
	const [dateSelected, setDateSelected] = useState('');
	const handleChangeDate = e => {
		setDateSelected(e.target.value);
	};
	const ITEM_HEIGHT = 48;
	const ITEM_PADDING_TOP = 8;
	const MenuProps = {
		PaperProps: {
			style: {
				maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
				width: 250
			}
		}
	};
	const columns = [
		{ field: 'id', headerName: 'ID', width: 70 },
		{
			field: 'name',
			headerName: 'Nom Livre',
			width: 650
		},
		{
			field: 'actions',
			headerName: 'Actions',
			width: 230,
			renderCell: params => {
				return (
					<div>
						<Link to={`/books/`}>Voir Détail</Link>
					</div>
				);
			}
		}
	];

	const rows = [
		{
			id: 9,
			name: 'La mort'
		}
	];
	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center w-full">
					<div className="pt-10 pb-10">
						<div className="flex items-center">
							<Comment className="mr-4" />
							<h2>Commentaires</h2>
						</div>
					</div>
				</div>
			}
			content={
				<div style={{ height: 400, width: '100%', textAlign: 'center' }}>
					<div className="flex flex-1 justify-between items-center w-full p-8">
						<h2>Liste des réclamations</h2>
						<FormControl sx={{ m: 1 }} variant="standard">
							<InputLabel id="demo-customized-select-label">Sélectionner la date</InputLabel>
							<Select
								labelId="demo-customized-select-label"
								value={dateSelected}
								onChange={handleChangeDate}
								style={{
									width: '200px',
									textAlign: 'left'
								}}
								id="demo-customized-select"
							>
								<MenuItem value="mars">Mars 2021</MenuItem>
								<MenuItem value="avril">Avril 2021</MenuItem>
								<MenuItem value="september">September 2021</MenuItem>
							</Select>
						</FormControl>
					</div>
					<DataGrid
						disableSelectionOnClick
						rows={rows}
						columns={columns}
						pageSize={5}
						rowsPerPageOptions={[5]}
						checkboxSelection
					/>
				</div>
			}
		/>
	);
};

export default Comments;
