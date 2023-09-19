import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';

import http from '../http-commons';

const initialState = {
	authorsWithoutAccount: [],
	loading: null,
	error: false
};
export const getAuthorsWithoutAccount = createAsyncThunk(
	'authorWihtoutAccountrequest/get',
	async () => {
		const res = await http.get('/author/getAll');

		return res.data.data;
	}
);

export const createAuthorWithoutAcount = createAsyncThunk(
	'authorWihtoutAccountrequest/post',
	async formData => {
		await http.post('/author/add', formData);
	}
);

export const deleteAuthorWithoutAccount = createAsyncThunk('authorWihtoutAccountrequest/delete',
async (id) => {
	await http.delete(`/author/delete?id=${id}`);
}
)
export const updateAuthorWithoutAccount = createAsyncThunk(
	'authorWihtoutAccountrequest/put',
	async formData => {
		await http.put('/author/update', formData, {
			headers: {
				Authorization: `Bearer ${localStorage.getItem('token')}`
			}
		});
	}
);

const authorWithoutAccountSlice = createSlice({
	name: 'authorWithoutAccount',
	initialState,
	extraReducers: {
		[getAuthorsWithoutAccount.pending]: state => {
			state.loading = true;
		},
		[getAuthorsWithoutAccount.fulfilled]: (state, { payload }) => {
			state.authorsWithoutAccount = payload;
			state.loading = false;
		},
		[getAuthorsWithoutAccount.rejected]: state => {
			state.loading = false;
			state.error = true;
		},
		[createAuthorWithoutAcount.fulfilled]: (state, { payload }) => {
			state.error = false;
			state.loading = false;
		},
		[createAuthorWithoutAcount.rejected]: (state, { payload }) => {
			state.loading = false;
			state.error = true;
		},
		[updateAuthorWithoutAccount.fulfilled]: (state, { payload }) => {
			state.error = false;
			state.loading = false;
		},
		[updateAuthorWithoutAccount.rejected]: (state, { payload }) => {
			state.loading = false;
			state.error = true;
		},
		[deleteAuthorWithoutAccount.fulfilled]: (state, { payload }) => {
			state.loading = false;
			state.error = true;
		},

	}
});

export default authorWithoutAccountSlice.reducer;
