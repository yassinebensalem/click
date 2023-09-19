import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import http from '../http-commons';

const initialState = {
	countries: [],
	loading: null,
	error: false
};
export const getCountries = createAsyncThunk('country/get', async joinRequest => {
	const res = await http.get('/country/getAll');

	return res.data;
});

const authorJoinSlice = createSlice({
	name: 'country',
	initialState,

	extraReducers: {
		[getCountries.pending]: state => {
			state.loading = true;
		},
		[getCountries.fulfilled]: (state, { payload }) => {
			state.countries = payload.data;
			state.loading = null;
		},
		[getCountries.rejected]: state => {
			state.loading = null;
			state.error = true;
		}
	}
});

export default authorJoinSlice.reducer;
