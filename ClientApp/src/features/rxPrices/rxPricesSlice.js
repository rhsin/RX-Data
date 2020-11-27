import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import lowercaseKeys from 'lowercase-keys';
import { API_URL } from '../../constants';

export const fetchRxPrices = createAsyncThunk('rxPrices/fetchRxPrices', async () => {
  const response = await axios.get(API_URL + 'rxPrices');
  return response.data;
});

export const findRxPrices = createAsyncThunk('rxPrices/findRxPrices', async (name) => {
  const response = await axios.get(`${API_URL}rxPrices/find/${name}`);
  const rxPrices = await response.data.rxPrices;
  return rxPrices.map(rxPrice => lowercaseKeys(rxPrice));
});

export const rxPricesSlice = createSlice({
  name: 'rxPrices',
  initialState: { entities: [], status: 'idle', error: null },
  extraReducers: {
    [fetchRxPrices.pending]: (state, action) => {
      state.status = 'loading';
    },
    [fetchRxPrices.fulfilled]: (state, action) => {
      state.status = 'succeeded';
      state.entities = action.payload;
    },
    [fetchRxPrices.rejected]: (state, action) => {
      state.status = 'failed';
      state.error = action.error.message;
    },
    [findRxPrices.fulfilled]: (state, action) => {
      state.entities = action.payload;
    }
  }
});

export const selectRxPrices = state => state.rxPrices.entities;
export const selectStatus = state => state.rxPrices.status;
export const selectError = state => state.rxPrices.error;

export default rxPricesSlice.reducer;