using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DeviseHR_Server.Middleware
{
    public class AuthMW
    {
        

        public static async Task Invoke(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Please Authenticate");
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("Error");
            }
        }

   
    }
}



//import { NextFunction, Request, Response } from 'express';
//import { verify } from 'jsonwebtoken';
//import { IAuthenticatedUserRequest } from '../Types/Request_Types/User_Types';
//import { DecodedToken } from '../Types/General_Types';

//const auth = async(
//  req: IAuthenticatedUserRequest,
//res: Response,
//  next: NextFunction
//) => {
//  try {
//    const token = req.header('Authorization')?.replace('Bearer ', '');
//if (!token) throw new Error('Please Authenticate');
//const decode = (await verify(
//  token,
//  process.env.JWT_SECRET!
//)) as DecodedToken;

//req.decodedUser = decode;
//req.userId = Number(decode.id);
//req.companyId = Number(decode.companyId);
//next();
//  } catch (err: any) {
//    res.status(400).send({
//    data: null,
//      success: false,
//      message: err.message,
//    });
//}
//};

//export default auth;
