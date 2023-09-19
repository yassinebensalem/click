

import React from 'react';
import { Paper, makeStyles } from '@material-ui/core';
import moment from 'moment';
import { Link } from 'react-router-dom';
import 'moment/locale/fr';

import firstImage from '../../../images/first.png';
import secondImage from '../../../images/second.png';
import thirdImage from '../../../images/third.png';
import trophyImage from '../../../images/Trophy.png';

const GradientPaper = ({
  header,
  content,
  bgColor,
  image,
  vector,
  variant,
  height,
}) => {
  const gradient =
  bgColor || 'linear-gradient(94.2deg, #FFFFFF 0%, #EFF2FA 49.96%)';
  const customHeight = height || 194;
  
  
    const useStyles = makeStyles(() => ({
      paper: {
        height: customHeight,
        background: gradient,
        display: 'flex',
        flexDirection: 'column',
        overflow: 'hidden',
        boxShadow: `0px 2px 1px -1px rgba(0,0,0,0.2),0px 1px 1px 0px rgba(0,0,0,0.14),0px 1px 3px 0px rgba(0,0,0,0.12)`,
        flex:'auto',
        transition: "boxShadow 1s",
        "&:hover": {
          boxShadow: "#000",
        }
      },
      firstText: {
        flex: 1,
        fontStyle: 'normal',
        fontWeight: 600,
        fontSize: '20px',
        lineHeight: '24px',
        color: '#5D586C',
      },
      secondText: {
        flex: 'none',
        fontStyle: 'normal',
        fontWeight: 400,
        fontSize: '15px',
        lineHeight: '20px',
        color: 'rgb(165 162 173)',
        textAlign: 'right',
      },
      body: {
        display: 'flex',
        alignItems: 'center',
        padding: '0px',
        flexGrow: 1,
      },
      content: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'flex-start',
        padding:'34px 24px 24px 24px',
        flex: '1 0 50%',
      },
      statsContent : {
        display: 'flex',
        flex: 'auto',
        padding: '34px 24px 24px 24px',
        alignItems: 'flex-start',
        justifyContent: 'space-between',
      },
      list: {
        display: 'flex',
        alignItems: 'center',
        gap: '16px',
        marginBottom: '16px',
      },
      icon: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        width: '50px',
        height: '50px',
        background: 'rgba(106, 109, 178, 0.08)',
        borderRadius: '50%',
      },
      vector: {
        width: '32px',
        height: '23px',
      },
      textContainer: {
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'flex-start',
      },
      firstChildText: {
        fontStyle: 'normal',
        fontWeight: 600,
        fontSize: '20px',
        lineHeight: '24px',
        marginBottom: '4px',
        color:'#797cba',
      },
      secondChildText: {
        fontStyle: 'normal',
        fontWeight: 400,
        fontSize: '15px',
        lineHeight: '20px',
      },
      image: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        flex: '1 0 50%',
        maxHeight: '100%',
      },
      trophy: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        width: '35%',
        marginRight: '-15%',
      },
      statsIcon: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        width: 'auto',
        height: 'auto',
        background: 'rgba(106, 109, 178, 0.08)',
        borderRadius: '50%',
      },
      statsVector: {
        padding: '9px'
      }
    }));
    const classes = useStyles();
  


  const renderCardHeader = (variant) => {
    if (variant === 'large') {
      return (
        <div className='flex items-center p-24'>
          <div className='flex flex-col items-start gap-2 flex-1'>
            <div className={classes.firstText}>{header.title}</div>
            <div className='text-gray-500 font-roboto text-sm font-normal leading-5'>
              {header.subtitle}
            </div>
          </div>
          <Link
            style={{
              flex: 'none',
              color: '#A5A2AD',
              fontSize: '15px',
              fontWeight: 'normal',
              textAlign: 'right',
              textDecoration: 'underline',
            }}
          >
            Voir la liste complète
          </Link>
        </div>
      );
    } else {
      return (
        <div className='flex items-center p-24'>
          <div className={classes.firstText}>{header.title}</div>
          <div className={classes.secondText}>{`Aujourd’hui ${moment().format(
            'HH:mm'
          )}`}</div>
        </div>
      );
    }
  };

  const renderCardBody = (variant) => {
    if (variant === 'large' && content) {
      return (
        <div className={classes.body}>
          <div className='flex p-24 items-start gap-18 self-stretch flex-grow'>
            <ul className='flex-auto max-w-4/5'>
              {content.map((item, index) => (
                <li
                  className='flex items-center gap-16 flex-1 self-stretch mb-16 mr-16'
                  key={index}
                >
                  <span className={index < 3 ? 'flex p-6 items-start rounded-6 bg-white bg-opacity-80 my-10 mr-16' : 'text-right font-roboto text-15 font-normal leading-22'}>
                    <div className={index < 3 && 'w-22 h-22'}>
                      {index < 3 && <img src={index === 0 ? firstImage : index === 1 ? secondImage : index === 2 ? thirdImage : ''} alt={`Image ${index + 1}`} />}
                      {index === 3 && '4.'}
                    </div>
                  </span>
                  {!item.publisherId ? (
                    <span className='flex flex-col items-start flex-1 mr-10'>
                      <div className='flex flex-col justify-center self-stretch text-5d586c font-roboto text-15 font-semibold leading-21'>
                        {item.title}
                      </div>
                      <div className='flex flex-col justify-center self-stretch text-gray-500 font-roboto text-13 font-semibold leading-21'>
                        {item.authorName}
                      </div>
                    </span>
                  ) : (
                    <span className='flex flex-col items-start flex-1 mr-10'>
                      <div className='flex flex-col justify-center self-stretch text-5d586c font-roboto text-15 font-semibold leading-21'>
                        {item.raisonSociale}
                      </div>
                    </span>
                  )}
                  <span>{item.count}</span>
                </li>
              ))}
            </ul>
          </div>
          <div className={classes.trophy}>
            <img src={trophyImage} alt='trophy' />
          </div>
        </div>
      );
    } else if (variant === 'stats' && content) {
      return (
        <div className={classes.body}>
          <div className={classes.statsContent}>
            {content.map((item, index) => (
              <div className={classes.list} key={index}>
                <div className={classes.statsIcon}>
                  <div className={classes.statsVector}>{item.vector}</div>
                </div>
                <div className={classes.textContainer}>
                  <div className={classes.firstChildText}>{item.firstChildText}</div>
                  <div className={classes.secondChildText}>{item.secondChildText}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      );
    } else if (content) {
      return (
        <div className={classes.body}>
          <div className={classes.content}>
            {content.map((item, index) => (
              <div className={classes.list} key={index}>
                <div className={classes.icon}>
                  <div className={classes.vector}>{vector}</div>
                </div>
                <div className={classes.textContainer}>
                  <div className={classes.firstChildText}>{item.firstChildText}</div>
                  <div className={classes.secondChildText}>{item.secondChildText}</div>
                </div>
              </div>
            ))}
          </div>
          <div className={classes.image}>{image}</div>
        </div>
      );
    }
    return null;
  };

  return (
    <Paper className={classes.paper} elevation={1}>
      {renderCardHeader(variant)}
      {renderCardBody(variant)}
    </Paper>
  );
};

export default GradientPaper;
