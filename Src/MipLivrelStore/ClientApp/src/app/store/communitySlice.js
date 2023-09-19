import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import http from '../http-commons';

const initialState = {
    allCommunities: [],
    community: {},
    communityMembers: [],
    loading: false,
};

export const getAllCommunities = createAsyncThunk('community/getAll', async () => {
    const res = (await http.get('/community/getAll')).data; 
    return res.data; 
});

export const getCommunityById = createAsyncThunk('community/getById', async (id) => {
    const res = (await http.get(`/community/getById?id=${id}`)).data; 
    return res.data; 
});

export const getCommunityMembers = createAsyncThunk('community/getById', async (id) => {
    const res = await http.get(`/community/getById?id=${id}`); 
    return res.data; 
});

export const createCommunity = createAsyncThunk('community/Add', async (newCommunity) => {
    const res = await http.post('/community/Add', newCommunity); 
    return res.data;
});

export const updateCommunity = createAsyncThunk('community/update', async (updatedCommunity) => {
    const res = await http.put('/community/update', updatedCommunity); 
    return res.data; 
});

export const deleteCommunity = createAsyncThunk('community/delete', async (id) => {
    await http.delete(`/community/Delete?id=${id}`); 
    return id;
});

const communitySlice = createSlice({
    name: 'community',
    initialState,
    reducers: {
        resetArray: (state) => {
            state.communityMembers = initialState.communityMembers;
        },
    },
    extraReducers: {
        [getAllCommunities.pending]: (state) => {
            state.loading = true;
        },
        [getAllCommunities.fulfilled]: (state, { payload }) => {
            state.allCommunities = payload;
            state.loading = false;
        },
        [getCommunityById.pending]: (state) => {
            state.loading = true;
        },
        [getCommunityById.fulfilled]: (state, { payload }) => {
            state.community = payload;
            state.loading = false;
        },
        [getCommunityMembers.pending]: (state) => {
            state.loading = true
        },
        [deleteCommunity.pending]: (state) => {
            state.loading = true
        },
        [deleteCommunity.fulfilled]: (state, action) => {
            let communityId = action.meta.arg
            state.allCommunities = state.allCommunities.filter(community => community.id !== communityId)
        },
        // [getCommunityMembers.fulfilled]: (state, { payload }) => {
        //     state.loading = false;
        //     state.communityMembers = payload
        // }
    },
});

export const { resetArray } = communitySlice.actions;
export default communitySlice.reducer;
