import moment from "moment-jalaali";

/**
 * Formats a number with thousand separators.
 * @param {number} number - The number to format.
 * @returns {string} - The formatted number as a string.
 */
export const formatNumber = (number) => {
  return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};

/**
 * Converts an English date to Persian (Jalaali) and formats it.
 * @param {string} englishDate - The English date string (e.g., "2025-03-09T01:31:02.4049374").
 * @param {string} format - The desired Persian date format.
 * @returns {string} - The formatted Persian date.
 */
export const convertToPersianDate = (englishDate, format) => {
  const date = moment(englishDate);

  switch (format) {
    case "YYYY/MM/DD":
      return date.format("jYYYY/jMM/jDD"); // Format 1: 1403/11/03
    case "DD MMMM YYYY":
      return date.format("jD jMMMM jYYYY"); // Format 2: 03 اسفند 1403
    case "HH:mm:ss":
      return date.format("HH:mm:ss"); // Format 3: 01:31:02
    default:
      return date.format("jYYYY/jMM/jDD"); // Default format
  }
};
