import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_URL } from '../../constants';

export const fetchUsers = createAsyncThunk('users/fetchUsers', async () => {
  const response = await axios.get(`${API_URL}/users`);
  return response.data;
});

export const addRxPrice = createAsyncThunk('users/addRxPrice',
  async (rxPriceId, userId) => {
    const response = await axios.post(`${API_URL}/users/rxPrices/${rxPriceId}/1`);
    return response.data;
});

export const removeRxPrice = createAsyncThunk('users/removeRxPrice',
  async (rxPriceId, userId) => {
    const response = await axios.delete(`${API_URL}/users/rxPrices/${rxPriceId}/1`);
    return response.data;
});

export const usersSlice = createSlice({
  name: 'users',
  initialState: {
    entities: [],
    status: 'idle',
    message: null,
    error: null 
  },
  extraReducers: {
    [fetchUsers.pending]: (state, action) => {
      state.status = 'loading';
    },
    [fetchUsers.fulfilled]: (state, action) => {
      state.status = 'succeeded';
      state.entities = action.payload;
    },
    [fetchUsers.rejected]: (state, action) => {
      state.status = 'failed';
      state.error = action.error.message;
    },
    [addRxPrice.fulfilled]: (state, action) => {
      state.status = 'idle';
      state.message = action.payload;
    },
    [removeRxPrice.fulfilled]: (state, action) => {
      state.status = 'idle';
      state.message = action.payload;
    }
  }
});

export const selectUsers = state => state.users.entities;
export const selectStatus = state => state.users.status;
export const selectMessage = state => state.users.message;
export const selectError = state => state.users.error;

export default usersSlice.reducer;