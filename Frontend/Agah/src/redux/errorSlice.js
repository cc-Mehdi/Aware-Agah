import { createSlice } from "@reduxjs/toolkit";

const errorSlice = createSlice({
  name: "error",
  initialState: {
    message: null, // پیام خطا
  },
  reducers: {
    setError: (state, action) => {
      state.message = action.payload; // ذخیره خطا
    },
    clearError: (state) => {
      state.message = null; // پاک کردن خطا
    },
  },
});

export const { setError, clearError } = errorSlice.actions;
export default errorSlice.reducer;
