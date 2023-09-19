import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';
import axios from 'axios';
import http from '../http-commons';
import { getDefaultOptions } from '../http-commons';
import { async } from 'q';

const initialState = {
	books: [],
	booksCount: null,
	soldBooksCount: null,
	bestSellers: null,
	loading: null,
	loadingBooks: null,
	loadingBookCount: null,
	loadingSoldBookCount: null,
	loadingSaveBook: null,
	loadingUpdateBook: null,
};
export const getBooks = createAsyncThunk('book/get', async () => {
	const res = await http.get(`/book/getAll?skip=0&take=10`);
	return res.data.data;
});

export const getBestSellers = createAsyncThunk('/Dashboard/GetPagedTopSelledBooks?size=4', async () => {
	const res = await http.get('/Dashboard/GetPagedTopSelledBooks?size=4')
	return res.data
})

export const getTotalPublishedBooksCount = createAsyncThunk('/Dashboard/getTotalPublishedBooksCount', async () => {
	const res = await http.get('/Dashboard/getTotalPublishedBooksCount');
	return res.data;
});

export const getTotalSoldBooksCount = createAsyncThunk('/Dashboard/GetFiltredSelledBooksCount', async () => {
	const res = await http.get('/Dashboard/GetFiltredSelledBooksCount')
	return res.data;
});

export const createBook = createAsyncThunk('book/post', async newBook => {

	const res = await http.post(`/book/Add`, newBook
	);
	return res.data.data;



});

export const updateBook = createAsyncThunk('book/put', async form => {
	const res = await http.put(`/book/Update`, form);
	return res.data.data;
});

export const filterBooks = createAsyncThunk('book/filter', async form => {
	const res = await http.post(`book/interval`, form);
	return res.data.data;
});

export const updateBookState = createAsyncThunk('book/updateState', async form => {
	const res = await http.put(`book/updatestate`, form);
	return res.data.data;
});

export const deleteBook = createAsyncThunk('book/delete', async id => {
	await http.delete(`book/delete?id=${id}`);
})

const bookSlice = createSlice({
	name: 'book',
	initialState,
	extraReducers: {
		[getTotalPublishedBooksCount.pending]: state => {
			state.loadingBookCount = true
		},
		[getTotalPublishedBooksCount.fulfilled]: (state, { payload }) => {
			state.booksCount = payload;
			state.loadingBookCount = false;
		},
		[getTotalSoldBooksCount.pending]: state => {
			state.loadingSoldBookCount = true
		},
		[getTotalSoldBooksCount.fulfilled]: (state, { payload }) => {
			state.soldBooksCount = payload;
			state.loadingSoldBookCount = false;

		},
		[getBooks.pending]: state => {
			state.loadingBooks = true;
		},
		[getBooks.fulfilled]: (state, { payload }) => {
			state.books = payload;
			state.loadingBooks = false;
		},

		[getBooks.rejected]: state => {
			state.loadingBooks = false;
		},
		[getBestSellers.rejected]: state => {
			state.loadingBooks = false;
		},
		[getBestSellers.fulfilled]: (state, {payload}) => {
			state.bestSellers = payload.items
		},
		[createBook.pending]: state => {
			state.loadingSaveBook = true;
		},
		[createBook.fulfilled]: state => {
			state.loadingSaveBook = false;
		},
		[createBook.rejected]: state => {
			state.loadingSaveBook = false;
		},
		[updateBook.fulfilled]: state => {
			state.loadingUpdateBook = false;
		},
		[updateBook.rejected]: state => {
			state.loadingUpdateBook = false;
		},
		[filterBooks.fulfilled]: state => {
			state.loading = true;
		},
		[filterBooks.fulfilled]: (state, { payload }) => {
			state.loading = false;
			state.books = payload;
		},
		[filterBooks.rejected]: state => {
			state.loading = false;
		},
		[updateBookState.pending]: (state, { payload }) => {
			state.loading = true;
		},
		[updateBookState.fulfilled]: (state, { payload }) => {
			state.loading = false;
		},
		[updateBookState.rejected]: (state, { payload }) => {
			state.loading = false;
		},


	}
});

export default bookSlice.reducer;
