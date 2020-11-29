import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import axios from 'axios';
import lowercaseKeys from 'lowercase-keys';
import { API_URL } from '../../constants';

export const fetchRxPrices = createAsyncThunk('rxPrices/fetchRxPrices', async (url) => {
  const response = await axios.get(url);
  return response.data;
});

export const findRxPrices = createAsyncThunk('rxPrices/findRxPrices', async (name) => {
  const response = await axios.get(`${API_URL}/rxPrices/find/${name}`);
  const rxPrices = await response.data.rxPrices;
  return rxPrices.map(rxPrice => lowercaseKeys(rxPrice));
});

export const rxPricesSlice = createSlice({
  name: 'rxPrices',
  initialState: {
    entities: [],
    status: 'idle',
    error: null 
  },
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
    [findRxPrices.pending]: (state, action) => {
      state.status = 'loading';
    },
    [findRxPrices.fulfilled]: (state, action) => {
      state.status = 'succeeded';
      state.entities = action.payload;
    }
  }
});

export const selectRxPrices = state => state.rxPrices.entities;
export const selectStatus = state => state.rxPrices.status;

export const rxPricesUrl = `${API_URL}/rxPrices`;
export const webRxPricesUrl = (name) => `${API_URL}/rxPrices/Fetch/Canada/${name}`;
export const findRxPricesUrl = (name) => `${API_URL}/rxPrices/Price/Mg/${name}`;

export default rxPricesSlice.reducer;