import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';

import http from '../http-commons';

const initialState = {
	authors: [],
	loadingAuthors: null,
	loadingSaveAuthor: null,
	loadingUpdateAuthor: null,
	loading: null,
	error: false, 
	isUpdated: false,
	sendMail: false
};
export const getAuthors = createAsyncThunk('authorrequest/get', async () => {
	const res = await http.get('/account/UsersByRole?role=author');

	return res.data.data.usersList;
});

export const createAuthor = createAsyncThunk(
	'authorrequest/post',
	async formData => {
		await http.post('/account/register', formData);
	}
);

export const updateAuthor = createAsyncThunk(
	'authorrequest/put',
	async formData => {
		await http.put('/account/update', formData, {
			headers: {
				Authorization: `Bearer ${localStorage.getItem('token')}`
			}
		});
	}
);

export const deleteAuthor = createAsyncThunk('authorrequest/delete',
async (id) => {
	await http.delete(`/account/delete?id=${id}`);
}
)


export const getActiveUsersByRole = createAsyncThunk('authorrequest/getActiveUsers', async(role) => {
	await http.get(`/account/ActiveUsersByRole?role=${role}`);
})

export const getActiveAuthorsByRole = createAsyncThunk('authorrequest/getActiveAuthor', async() => {
	const res = await http.get(`/account/ActiveUsersByRole?role=author`);
	return res.data.data.usersList;
});

export const sendMail = createAsyncThunk('editorrequest/send-mail', async(mail) => {
	await http.post(`/account/sendEmail`, mail);
})
const authorSlice = createSlice({
	name: 'author',
	initialState,
	extraReducers: {
		[getAuthors.pending]: state => {
			state.loadingAuthors = true;
		},
		[getAuthors.fulfilled]: (state, { payload }) => {
			state.authors = payload;
			state.loadingAuthors = false;
		},
		[getAuthors.rejected]: state => {
			state.loadingAuthors = false;
			state.error = true;
		},
		[createAuthor.fulfilled]: (state, { payload }) => {
			state.error = false;
			state.loadingSaveAuthor = false;
		},
		[createAuthor.pending]: (state, {payload}) => {
			state.error = false;
			state.loadingSaveAuthor = true;
		},
		[createAuthor.rejected]: (state, { payload }) => {
			state.loadingSaveAuthor = false;
			state.error = true;
		},
		[updateAuthor.pending]: (state) => {
			state.isUpdated= false
			state.loadingUpdateAuthor=true
		},
		[updateAuthor.fulfilled]: (state) => {
			state.isUpdated= true
			state.loadingUpdateAuthor=false
		},
		[deleteAuthor.fulfilled]: (state) => {
			state.loading = false;
		},
		[getActiveAuthorsByRole.fulfilled]: (state, {payload}) => {
			state.authors = payload;
			state.isLoading = false;
		},
		[sendMail.fulfilled]: (state, {payload}) => {
			state.isLoading = false;
			state.error = false;
			state.isUpdated = true;
		},
	}
});

export default authorSlice.reducer;
