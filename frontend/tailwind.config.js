/** @type {import('tailwindcss').Config} */
module.exports = {
	content: [
	  "./src/**/*.{html,ts}",
	],
	theme: {
	  extend: {
		fontFamily: {
			gilroy: ['Gilroy', 'sans-serif'],  // Add Gilroy font to Tailwind theme
		  },
		  fontWeight: {
			thin: 100,
			ultraLight: 200,
			light: 300,
			regular: 400,
			medium: 500,
			semiBold: 600,
			bold: 600,
			extraBold: 800,
			heavy: 900,
		  },
		colors: {
		  secondary: '#9333ea',
		  'brand-green': '#80C522',
		  'dark-green': '#5B6056',
		  primary: '#88C533',    // 88C533
		  darkGray: '#494949',   // 494949
		  almostBlack: '#191B19',// 191B19
		  mediumGray: '#3C3D3C', // 3C3D3C
		  lightGray: '#82867E',  // 82867E
		}
	  }
	},
	plugins: [
		require('tailwind-scrollbar'),
	],
}
