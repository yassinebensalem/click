import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';
import axios from 'axios';
import http from '../http-commons';
import swal from 'sweetalert';


const initialState = {
	authorsJoined: [],
	loading: null,
	updated: false,
	error: false,
	dateDebut: '',
	dateFin: ''
};
export const getAuthorsToJoin = createAsyncThunk('authorJoinRequest/get', async joinRequest => {
	const res = await http.post('/joinrequest/interval', joinRequest, {
		headers: {
			Authorization: `Bearer ${localStorage.getItem('token')}`
		}
	});
	return res.data;
});

export const updateJoinRequest = createAsyncThunk('authorJoinRequest/put', async joinRequest => {
	const res = await http.put('/joinrequest/Update', joinRequest, {
		headers: {
			Authorization: `Bearer ${localStorage.getItem('token')}`
		}
	});

	return res.data.data;
});
const authorJoinSlice = createSlice({
	name: 'authorJoin',
	initialState,
	reducers: {
		reset: () => initialState,
		setDateDebut: (state, { payload }) => {
			state.dateDebut = payload;
		},
		setDateFin: (state, { payload }) => {
			state.dateFin = payload;
		}
	},
	extraReducers: {
		[getAuthorsToJoin.pending]: state => {
			state.loading = true;
			state.updated = false;
		},
		[getAuthorsToJoin.fulfilled]: (state, { payload }) => {
			state.authorsJoined = payload.data;
			state.loading = null;
			state.updated = false;
		},
		[getAuthorsToJoin.rejected]: state => {
			state.loading = null;
			state.error = true;
			state.updated = false;
		},
		[updateJoinRequest.fulfilled]: (state, { payload }) => {
			state.updated = true;
			const arr = current(state.authorsJoined);
			const arrMapped = arr.map(obj => {
				const copyObj = { ...obj };

				if (copyObj.id === payload.id) {
					if (payload.status === 1) {
						copyObj.status = 1;
					} else if (payload.status === 2) {
						copyObj.status = 2;
					} else {
						copyObj.status = 1;
					}
				}
				return copyObj;
			});
			state.authorsJoined = [...arrMapped];
		},
		[updateJoinRequest.rejected]: state => {
			state.loading = null;
			state.error = true;
			state.updated = false;
		}
	}
});

export const { reset, getAll, setDateDebut, setDateFin } = authorJoinSlice.actions;

export default authorJoinSlice.reducer;
