import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import http from '../http-commons';

const initialState = {
    prizes: [],
    loading: null,
    prizedBooks: [],
    booksByTitle: []
};

export const getPrizes = createAsyncThunk('prizes/get', async () => {
    const res = await http.get(`/prized/getAll`);
    return res.data.data;
});

export const addNewPrize = createAsyncThunk('prizes/post', async (newPrize) => {
    const res = await http.post(`/prized/addprize`, newPrize);
    return res.data.data;
});
export const getBookByTitle = createAsyncThunk('prizes/getBook', async (Book) => {
    const res = await http.post(`/prized/getBook`, Book);
    return res.data.data;
});

export const addPrizeBook = createAsyncThunk('prizes/postPrizeBook', async (Prize) => {
    const res = await http.post(`/prized/addPrizeBook`, Prize);
    return res.data.data;
});

export const getAllPrizedBooks = createAsyncThunk('prizes/getAllPrizeBook', async (Prize) => {
    const res = await http.get(`/prized/getAllPrizeBook`);
    return res.data.data;
});
const prizeSlice = createSlice({
    name: 'prize',
    initialState,
    reducers: {
        reset: (state) => {
            state.prizedBooks = initialState.prizedBooks
        }
    },
    extraReducers: {
        [getPrizes.pending]: state => {
            state.loading = true;
        },
        [getPrizes.fulfilled]: (state, { payload }) => {
            state.prizes = payload;

            state.loading = null;
        },
        [getPrizes.rejected]: state => {
            state.loading = null;
        },
        [addNewPrize.pending]: state => {
            state.loading = true;
        },
        [addNewPrize.fulfilled]: (state, { payload }) => {
            state.loading = false;
        },
        [addNewPrize.rejected]: (state) => {
            state.loading = false;
        },
        [getBookByTitle.pending]: (state) => {
            state.loading = true;
        },
        [getBookByTitle.fulfilled]: (state, { payload }) => {
            state.loading = false;
            state.booksByTitle = payload;
        },
        [getBookByTitle.rejected]: (state) => {
            state.loading = false;
        },
        [addPrizeBook.pending]: (state) => {
            state.loading = true;
        },
        [addPrizeBook.fulfilled]: (state, { payload }) => {
            state.loading = false;
        },
        [addPrizeBook.rejected]: (state) => {
            state.loading = false;
        },
        [getAllPrizedBooks.pending]: (state) => {
            state.loading = true;
        },
        [getAllPrizedBooks.fulfilled]: (state, { payload }) => {
            state.loading = false;
            state.prizedBooks = payload;
        },
        [getAllPrizedBooks.rejected]: (state) => {
            state.loading = false;
        }
    }
});

export const { reset } = prizeSlice.actions;

export default prizeSlice.reducer;
