import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';

import http from '../http-commons';

const initialState = {
	subscribers: [],
	loadingSubscribers: null,
	loadingSaveSubscriber: null,
	loadingUpdateSubscriber: null,
	loading: null,
	error: false,
	isUpdated: false,
	numberOfActiveSubscribers: 0,
	nonConfirmedSubscribers: null
};
export const getSubscribers = createAsyncThunk('subscriberrequest/get', async () => {
	const res = await http.get('/account/UsersByRole?role=subscriber');

	return res.data.data.usersList;
});

export const getNonConfirmedSubscribers = createAsyncThunk('NonConfirmedSubscriptionsCount', async () => {
	const res = await http.get('/account/NonConfirmedSubscriptionsCount');
	return res.data;
})

export const createSubscriber = createAsyncThunk('subscriberrequest/post', async formData => {
	await http.post('/account/register', formData);
});

export const updateSubscriber = createAsyncThunk('subscriberrequest/put', async formData => {
	await http.put('/account/update', formData, {
		headers: {
			Authorization: `Bearer ${localStorage.getItem('token')}`
		}
	});
});
export const deleteSubscriber = createAsyncThunk('subscriberrequest/delete',
async (id) => {
	await http.delete(`/account/delete?id=${id}`);
}
)

export const getActiveSubscribers = createAsyncThunk('subscriberrequest/getActiveSubscribers', async () => {
	const res = await http.get(`/account/ActiveCustomers`);

	return res.data.data;
})

const subscriberSlice = createSlice({
	name: 'subscriber',
	initialState,
	extraReducers: {
		[getSubscribers.pending]: state => {
			state.loadingSubscribers = true;
		},
		[getSubscribers.fulfilled]: (state, { payload }) => {
			state.loadingSubscribers = payload;
			state.loading = false;
			state.subscribers = payload;
		},
		[getSubscribers.rejected]: state => {
			state.loadingSubscribers = false;
			state.error = true;
		},
		[getNonConfirmedSubscribers.pending]: state => {
			state.loadingSubscribers = true;
		},
		[getNonConfirmedSubscribers.fulfilled]: (state, {payload}) => {
			state.nonConfirmedSubscribers = payload
		},
		[createSubscriber.fulfilled]: (state, { payload }) => {
			state.error = false;
			state.loadingSaveSubscriber = false;
		},
		[createSubscriber.rejected]: (state, { payload }) => {
			state.loadingSaveSubscriber = false;
			state.error = true;
		},
		[updateSubscriber.pending]: (state) => {
			state.isUpdated= false
			state.loadingUpdateSubscriber=false
		},
		[updateSubscriber.fulfilled]: (state) => {
			state.isUpdated= true
			state.loadingUpdateSubscriber=true
		},
		[deleteSubscriber.fulfilled]: (state) => {
			state.loading = false;
		},
		[getActiveSubscribers.fulfilled]: (state, {payload}) => {
			state.numberOfActiveSubscribers = payload;
		}
	}
});

export default subscriberSlice.reducer;
