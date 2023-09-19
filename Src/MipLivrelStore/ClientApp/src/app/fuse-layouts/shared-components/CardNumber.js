import React from 'react';
import { Card, Typography } from '@material-ui/core'
const CardNumber = ({ children }) => {
    return (
        <Typography
            style={{
                'fontSize': '70px',
                'fontWeight': '300',
                /*'background': "#88cf00",
                'display': 'flex',
                'justify-content': 'center',
                'align-items': 'center',
                'color': "#fff",
                borderRadius: "50%",
                width: '50px',
                height: '50px'*/

            }}
        >

            {children}
        </Typography>
    )
}

export default CardNumber;