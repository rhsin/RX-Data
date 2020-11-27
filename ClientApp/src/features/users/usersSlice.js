import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_URL } from '../../constants';

export const fetchUsers = createAsyncThunk('users/fetchUsers', async () => {
  const response = await axios.get(API_URL + 'users');
  return response.data;
});

export const usersSlice = createSlice({
  name: 'users',
  initialState: { entities: [], status: 'idle', error: null },
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
    }
  }
});

export const selectUsers = state => state.users.entities;
export const selectStatus = state => state.users.status;
export const selectError = state => state.users.error;

export default usersSlice.reducer;