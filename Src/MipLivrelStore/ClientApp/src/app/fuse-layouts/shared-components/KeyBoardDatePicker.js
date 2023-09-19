import React, { useState } from 'react';
import { KeyboardDatePicker } from '@material-ui/pickers';
const DatePicker = () => {
	const [selectedDate, handleDateChange] = useState(new Date());
	return (
		<KeyboardDatePicker
			autoOk
			variant="inline"
			inputVariant="outlined"
			label="Date de création"
			value={selectedDate}
			format="YYYY/MM/DD"
			InputAdornmentProps={{ position: 'start' }}
			onChange={date => handleDateChange(date)}
			className="w-full mt-10"
		/>
	);
};

export default DatePicker;
