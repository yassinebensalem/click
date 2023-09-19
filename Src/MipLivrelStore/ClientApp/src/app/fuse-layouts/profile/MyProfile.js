import React from 'react';
import {
    Paper,
    Avatar
} from '@material-ui/core';
import { useSelector } from 'react-redux';
import { makeStyles } from '@material-ui/core/styles';
import clsx from 'clsx';
const useStyles = makeStyles(theme => ({
    avatar: {
        width: 72,
        height: 72,
        position: 'absolute',
        top: 0,
        left: 10,
        padding: 8,
        background: '#80080',
        boxSizing: 'content-box',
        color: "#fff",
        transform: 'translateX(-50%)',
        transition: theme.transitions.create('all', {
            duration: theme.transitions.duration.shortest,
            easing: theme.transitions.easing.easeInOut
        }),

    }
}));
const MyProfile = () => {
    const { firstName, lastName, user } = useSelector(state => state.auth);
    const classes = useStyles();
    const stringToColor = (string) => {
        let hash = 0;
        let i;

        //         /* eslint-disable no-bitwise */
        for (i = 0; i < string.length; i += 1) {
            hash = string.charCodeAt(i) + ((hash << 5) - hash);
        }

        let color = '#';

        for (i = 0; i < 3; i += 1) {
            const value = (hash >> (i * 8)) & 0xff;
            color += `00${value.toString(16)}`.substr(-2);
        }         /* eslint-enable no-bitwise */

        return color;
    }
    const stringAvatar = (name) => {
        return {
            sx: {
                bgcolor: stringToColor(name),
            },
            children: `${name.split(' ')[0][0]}${name.split(' ')[1][0]}`,
        };
    }
    return (
        <Paper
            style={{
                width: '60%',
                marginLeft: 'auto',
                marginRight: 'auto',
                padding: "20px",
                marginTop: "10px",
                height: "200px",
                position: "relative",
            }}
        >
            <div className="flex items-center">

                <Avatar className={clsx(classes.avatar, 'avatar')} {...stringAvatar(`${firstName} ${lastName}`)} />
                <div className="flex flex-col"
                    style={{
                        marginLeft: "80px",
                        textAlign: "left"
                    }}
                >
                    <h2 className="mb-5 font-bold">{`${firstName} ${lastName}`}</h2>
                    <div className="flex items-end mb-5">
                        <h3 className="font-semibold">Email: </h3>
                        <span>{user}</span>
                    </div>
                    <div className="flex items-end mb-5">
                        <h3 className="font-semibold">Nom: </h3>
                        <span>{lastName}</span>
                    </div>
                    <div className="flex items-end mb-5">
                        <h3 className="font-semibold">Prénom: </h3>
                        <span>{firstName}</span>
                    </div>
                </div>
            </div>

        </Paper>
    )
}

export default MyProfile
