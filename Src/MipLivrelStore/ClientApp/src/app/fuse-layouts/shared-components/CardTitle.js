import React from 'react';
import { Card, Typography } from '@material-ui/core'
const CardTitle = ({ children }) => {
    return (
        <Typography
            style={{
                'fontWeight': 'bold',
                'fontSize': '14px',
                'color': 'rgb(160, 160, 160)'
            }}
        >
            {children}
        </Typography>
    )
}

export default CardTitle;