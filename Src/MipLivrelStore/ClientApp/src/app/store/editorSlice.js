import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';

import http from '../http-commons';


const initialState = {
	editors: [],
	editorsCount: null,
	bestEditors: null,
	loadingEditors: null,
	loadingSaveEditor: null,
	loadingUpdateEditor: null,
	loading: null,
	error: false,
	isUpdated: false,
	firstEditorId: ''
};
export const getEditors = createAsyncThunk('editorrequest/get', async () => {
	const res = await http.get('/account/UsersByRole?role=editor');

	return res.data.data.usersList;
});

export const createEditor = createAsyncThunk(
	'editorrequest/post',
	async formData => {
		await http.post('/account/register', formData);
	}
);

export const updateEditor = createAsyncThunk(
	'editorrequest/put',
	async formData => {
		await http.put('/account/update', formData, {
			headers: {
				Authorization: `Bearer ${localStorage.getItem('token')}`
			}
		});
	}
);

export const deleteEditor = createAsyncThunk('editorrequest/delete',
async (id) => {
	await http.delete(`/account/delete?id=${id}`);
}
)

export const getEditorsCount = createAsyncThunk('/Dashboard/GetEditorsCount', async () => {
	const res = await http.get('/Dashboard/GetEditorsCount')
	return res.data;
})

export const getBestEditors = createAsyncThunk('/Dashboard/GetPagedTopPublishers?index=0&size=3', async () => {
	const res = await http.get('/Dashboard/GetPagedTopPublishers?index=0&size=3')
	return res.data
})

export const sendMail = createAsyncThunk('editorrequest/send-mail', async(mail) => {
	await http.post(`/account/sendEmail`, mail);
})

export const getActiveEditorsByRole = createAsyncThunk('authorrequest/getActiveEditors', async() => {
	const res = await http.get(`/account/ActiveUsersByRole?role=editor`);
	return res.data.data.usersList;
})
const editorSlice = createSlice({
	name: 'editor',
	initialState,
	reducers:{
       findFirstEditorId: (state, action) => {
		state.firstEditorId = action.payload[1] &&action.payload[1].id
	   }
	},
	extraReducers: {
		[getEditors.pending]: state => {
			state.loadingEditors =true;
		},
		[getEditors.fulfilled]: (state, { payload }) => {
			state.editors = payload;
			state.loadingSaveEditor = false;
		},
		[getEditors.rejected]: state => {
			state.loadingEditors = false;
			state.error = true;
		},

		[getBestEditors.pending]: state => {
			state.loadingEditors =true;
		},
		[getBestEditors.fulfilled]: (state, { payload }) => {
			state.bestEditors = payload.items;
			state.loadingSaveEditor = false;
		},
		[getBestEditors.rejected]: state => {
			state.loadingEditors = false;
			state.error = true;
		},
		[getEditorsCount.pending]: state => {
			state.loadingEditors =true;
		},
		[getEditorsCount.fulfilled]: (state, { payload }) => {
			state.editorsCount = payload;
			state.loadingSaveEditor = false;
		},
		[createEditor.fulfilled]: (state, { payload }) => {
			state.error = false;
			state.loadingSaveEditor = false;
		},
		[createEditor.rejected]: (state, { payload }) => {
			state.loadingSaveEditor = false;
			state.error = true;
		},

		[updateEditor.fulfilled]: (state) => {
			state.isUpdated= true;
			state.loadingUpdateEditor = false;
		},
		[updateEditor.pending]: (state) => {
			state.isUpdated= false;
			state.loadingUpdateEditor = true;
		},
		[deleteEditor.fulfilled]: (state) => {
			state.loading = false;
		},
		[sendMail.fulfilled]: (state) => {
			state.loading = false;
			state.error = false;
			state.isUpdated = true;
		},
		[getActiveEditorsByRole.fulfilled] : (state, {payload}) => {
			state.editors = payload;
			state.loading= false;
		}
	}
});
export const { findFirstEditorId } = editorSlice.actions;
export default editorSlice.reducer;
