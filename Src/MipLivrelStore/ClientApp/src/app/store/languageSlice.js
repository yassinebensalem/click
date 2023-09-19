import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import http from '../http-commons';

const initialState = {
	languages: [],
	loading: null,
	error: false
};
export const getLanguages = createAsyncThunk('languages/get', async () => {
	const res = await http.get('/language/getAll');

	return res.data.data;
});

const languageSlice = createSlice({
	name: 'language',
	initialState,
	extraReducers: {
		[getLanguages.pending]: state => {
			state.loading = true;
		},
		[getLanguages.fulfilled]: (state, { payload }) => {
			state.languages = payload;
			state.loading = null;
		},
		[getLanguages.rejected]: state => {
			state.loading = null;
			state.error = true;
		}
	}
});

export default languageSlice.reducer;
