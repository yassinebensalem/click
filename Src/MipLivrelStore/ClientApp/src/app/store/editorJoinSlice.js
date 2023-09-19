import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';
import axios from 'axios';
import http from '../http-commons';
import { getDefaultOptions } from '../http-commons';
const initialState = {
	editorsJoined: [],
	loading: null,
	updated: false,
	error: false,
	dateDebut: '',
	dateFin: ''
};
export const getEditorsToJoin = createAsyncThunk('editorJoinRequest/get', async joinRequest => {
	const res = await http.post('/joinrequest/interval', joinRequest);
	return res.data;
});

export const updateEditorJoin = createAsyncThunk('editorJoinRequest/put', async joinRequest => {
	const res = await http.put('/joinrequest/Update', joinRequest);
	return res.data.data;
});
const editorJoinSlice = createSlice({
	name: 'editorJoin',
	initialState,
	reducers: {
		reset: () => initialState,
		setDateDebut: (state, { payload }) => {
			state.dateDebut = payload;
		},
		setDateFin: (state, { payload }) => {
			state.dateFin = payload
		}
	},

	extraReducers: {
		[getEditorsToJoin.pending]: state => {
			state.loading = true;
		},
		[getEditorsToJoin.fulfilled]: (state, { payload }) => {
			state.editorsJoined = payload.data;
			state.loading = null;
		},
		[getEditorsToJoin.rejected]: state => {
			state.loading = null;
			state.error = true;
		},
		[updateEditorJoin.fulfilled]: (state, { payload }) => {
			state.updated = true;
			const arr = current(state.editorsJoined);
			const arrMapped = arr.map(obj => {
				const copyObj = { ...obj };
				if (copyObj.id === payload.id) {
					if (payload.status === 1) {
						copyObj.status = 1;
					} else if (payload.status === 2) {
						copyObj.status = 2;
					} else {
						copyObj.status = 3;
					}
				}
				return copyObj;
			});
			state.authorsJoined = [...arrMapped];
		},
		[updateEditorJoin.rejected]: state => {
			state.loading = null;
			state.error = true;
		}
	}
});

export const { reset, setDateDebut, setDateFin } = editorJoinSlice.actions;

export default editorJoinSlice.reducer;
