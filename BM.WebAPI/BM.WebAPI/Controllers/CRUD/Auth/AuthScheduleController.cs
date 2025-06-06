﻿using BM.Auth.ApplicationService.UserModule.Abtracts;
using BM.Auth.Dtos;
using BM.Constant;
using Microsoft.AspNetCore.Mvc;

namespace BM.WebAPI.Controllers.CRUD.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthScheduleController : Controller
    {
        private readonly IAuthScheduleService _authScheduleService;
        public AuthScheduleController(IAuthScheduleService authScheduleService)
        {
            _authScheduleService = authScheduleService;
        }
        [HttpPost("create")]
        [HttpPost]
        public async Task<IActionResult> AuthCreateSchedule([FromBody] List<AuthCreateScheduleDto> authCreateScheduleDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }

            try
            {
                var result = await _authScheduleService.AuthCreateSchedule(authCreateScheduleDtos);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "Không thể tạo lịch"));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorConst.Error(500, ex.Message));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> AuthUpdateSchedule([FromBody] AuthUpdateScheduleDto authUpdateScheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authScheduleService.AuthUpdateSchedule(authUpdateScheduleDto);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete/{scheduleID}")]
        public async Task<IActionResult> AuthDeleteSchedule(int scheduleID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authScheduleService.AuthDeleteSchedule(scheduleID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get/{scheduleID}")]
        public async Task<IActionResult> AuthGetSchedule(int scheduleID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authScheduleService.AuthGetSchedule(scheduleID);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getall")]
        public async Task<IActionResult> AuthGetAllSchedule()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authScheduleService.AuthGetAllSchedule();
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getEmpByDate")]
        public async Task<IActionResult> AuthGetEmpByDate(DateTime date , int branchesID, string typeOfEmp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorConst.Error(500, "Đầu vào không hợp lệ"));
            }
            try
            {
                var result = await _authScheduleService.GetEmployeesByDateAndBranch(date , branchesID , typeOfEmp);
                if (result == null)
                {
                    return BadRequest(ErrorConst.Error(500, "thông tin xác thực được cung cấp không chính xác"));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
