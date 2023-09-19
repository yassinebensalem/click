import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import http from '../http-commons';

const initialState = {
	categories: [],
	loading: null
};
export const getCategories = createAsyncThunk('category/get', async () => {
	const res = await http.get('/category/getAll');

	return res.data.data;
});

export const addCategory = createAsyncThunk('category/post', category => {
	http.post('/category/add', category);
});

export const removeCategory = createAsyncThunk('category/delete', (id) => {
	http.delete(`/category/delete?id=${id}`);
} );

export const updateCategory = createAsyncThunk('category/update', (category) => {
	http.put(`/category/update`, category);
})
const categorySlice = createSlice({
	name: 'category',
	initialState,

	extraReducers: {
		[getCategories.pending]: state => {
			state.loading = true;
		},
		[getCategories.fulfilled]: (state, { payload }) => {
			state.categories = payload;
			state.loading = false;
		},
		[getCategories.rejected]: state => {
			state.loading = false;
		},
		[addCategory.pending]: state => {
			state.loading = true;
		},
		[addCategory.fulfilled]: (state, { payload }) => {
			state.loading = false;
		},
		[addCategory.rejected]: state => {
			state.loading = false;
		},
		[removeCategory.pending]: state => {
			state.loading = true;
		},
		[removeCategory.fulfilled]: (state, {payload}) => {
			state.loading = false;
		},
		[removeCategory.rejected]: state => {
			state.loading = false;
		}
	}
});

export default categorySlice.reducer;
