import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import { API_URL } from '../../constants';

export const fetchRxPrices = createAsyncThunk('rxPrices/fetchRxPrices', async () => {
  const response = await axios.get(API_URL + 'RxPrices');
  return response.data;
});

export const rxPricesSlice = createSlice({
  name: 'rxPrices',
  initialState: { entities: [], status: 'idle', error: null },
  extraReducers: {
    [fetchRxPrices.pending]: (state, action) => {
      state.status = 'loading';
    },
    [fetchRxPrices.fulfilled]: (state, action) => {
      state.entities = action.payload;
      state.status = 'succeeded';
    },
    [fetchRxPrices.rejected]: (state, action) => {
      state.status = 'failed';
      state.error = action.error.message;
    }
  }
});

export const selectRxPrices = state => state.rxPrices.entities;
export const selectStatus = state => state.rxPrices.status;
export const selectError = state => state.rxPrices.error;

export default rxPricesSlice.reducer;