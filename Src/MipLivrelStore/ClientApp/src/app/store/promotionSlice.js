import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import http from '../http-commons';


const initialState = {
    promotions: [],
    promotion: {},
    promotionBooks: [],
    loading: false,
};
export const getAllPromotions = createAsyncThunk('promotion/get', async () => {
    const res = await http.get(`/book/AllPromotions`);
    return res.data.data;
});

export const addNewPromotion = createAsyncThunk('promotion/post', async (newPromotion) => {
    const res = await http.post(`/book/addPromotion`, newPromotion);
    return res.data.data;
});

export const getPromotionById = createAsyncThunk('promotion/getById', async (id) => {
    const res = await http.get(`/book/getPromotionById?id=${id}`);
    return res.data.data;
});
export const updatePromotion = createAsyncThunk('promotion/put', async (form) => {
    const res = await http.put(`/book/updatePromotion`, form);
    return res.data.data;
});
export const deletePromotion = createAsyncThunk('promotion/delete', async (id) => {
    await http.delete(`/book/deletePromotion?id=${id}`);   
});
export const addPromotionBook = createAsyncThunk('promotionBook/post', async(promotionBook) => {
    await http.post('/book/AddPromotionBook', promotionBook);
})

export const getAllPromotionBooks = createAsyncThunk('promotionBooks/get',async() => {
   const res =  await http.get('/book/AllPromotionBooks');
   return res.data.data
});
const promotionSlice = createSlice({
    name: 'promotion',
    initialState,
    reducers: {
        resetArray: (state) => {
            state.promotionBooks = initialState.promotionBooks
        }
    },
    extraReducers: {
        [getAllPromotions.pending]: state => {
            state.loading = true;
        },
        [getAllPromotions.fulfilled]: (state, { payload }) => {
            state.promotions = payload;
            state.loading = false;
        },
        [getAllPromotions.rejected]: state => {
            state.loading = false;
        },
        [getPromotionById.fulfilled] : (state, { payload }) => {
            state.promotion = payload;
            state.loading = false;
        },
   
        [addNewPromotion.pending]: state => {
            state.loading = true;
        },
        [addNewPromotion.fulfilled]: (state, { payload }) => {
            state.loading = false;
        },
        [addNewPromotion.rejected]: (state) => {
            state.loading = false;
        },
        [updatePromotion.pending]: state => {
            state.loading = true;
        },
        [updatePromotion.fulfilled]: (state, { payload }) => {
            state.loading = false;
        },
        [updatePromotion.rejected]: (state) => {
            state.loading = false;
        },
        [deletePromotion.pending]: state => {
            state.loading = true;
        },
        [deletePromotion.fulfilled]: (state, { payload }) => {
            state.loading = false;
        },
        [deletePromotion.rejected]: (state) => {
            state.loading = false;
        },
        [addPromotionBook.fulfilled]: (state) => {
            state.pending = true;
        },
        [addPromotionBook.fulfilled]: (state) => {
            state.loading = false;
        },
        [addPromotionBook.rejected]: (state) => {
            state.loading = false;
        },
        [getAllPromotionBooks.pending]: (state) => {
            state.pending = true;
        },
        [getAllPromotionBooks.fulfilled]: (state, {payload}) => {
            state.loading = false;
            state.promotionBooks = payload;
        },
        [getAllPromotionBooks.rejected]: (state) => {
            state.loading = false;
        },


    }
});

export const { resetArray } = promotionSlice.actions;
export default promotionSlice.reducer;