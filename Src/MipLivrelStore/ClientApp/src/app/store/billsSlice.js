import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import http from '../http-commons';

const initialState = {
    bills: [],
    loading: false
}

export const getInvoicesByEditor = createAsyncThunk('orderrequest/get', async (id) => {
     const res =  await http.get(`book/GetInvoicesByEditor?editorId=${id}`);
     return res.data.data
}) 
const billSlice = createSlice({
    name: 'bill',
    initialState,
    extraReducers: {
        [getInvoicesByEditor.fulfilled]: (state, {payload}) => {
            state.bills = payload;
            state.loading = false;
        },
        [getInvoicesByEditor.pending]: (state) => {
            state.loading = false;
        },
    }
});

export default billSlice.reducer;