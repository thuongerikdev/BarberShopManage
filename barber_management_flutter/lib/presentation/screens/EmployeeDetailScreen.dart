
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'package:barbermanagemobile/domain/usecases/get_emp_by_user_id_use_case.dart';

class EmployeeDetailScreen extends StatefulWidget {
  final int userID;

  const EmployeeDetailScreen({super.key, required this.userID});

  @override
  _EmployeeDetailScreenState createState() => _EmployeeDetailScreenState();
}

class _EmployeeDetailScreenState extends State<EmployeeDetailScreen> {
  final GetEmpByUserIDUseCase _getEmpByUserIDUseCase = GetIt.instance<GetEmpByUserIDUseCase>();
  Map<String, dynamic>? _employee;
  String? _error;
  bool _isLoading = true;

  // Define color constants at class level
  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);

  @override
  void initState() {
    super.initState();
    _fetchEmployeeDetails();
  }

  Future<void> _fetchEmployeeDetails() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final result = await _getEmpByUserIDUseCase.call(widget.userID);
      print('Fetched employee data for userID ${widget.userID}: $result');

      setState(() {
        _employee = {
          'empID': result['empID']?.toString() ?? 'N/A',
          'empCode': result['empCode']?.toString() ?? 'N/A',
          'positionID': result['positionID']?.toString() ?? 'N/A',
          'positionName': result['positionName']?.toString() ?? 'Nhân viên',
          'specialtyID': result['specialtyID']?.toString() ?? 'N/A',
          'specialtyName': result['specialtyName']?.toString() ?? 'Không có chuyên môn',
          'salary': result['salary']?.toString() ?? '0',
          'startDate': result['startDate']?.toString() ?? '',
          'userID': result['userID']?.toString() ?? widget.userID.toString(),
          'bonusSalary': result['bonusSalary']?.toString() ?? '0',
          'status': result['status']?.toString() ?? 'Không xác định',
          'branchID': result['branchID']?.toString() ?? 'N/A',
          'branchName': result['branchName']?.toString() ?? 'Không có chi nhánh',
          'image': result['image']?.startsWith('http') == true
              ? result['image']
              : 'https://via.placeholder.com/120',
          'email': result['email']?.toString() ?? 'Không có email',
          'phone': result['phone']?.toString() ?? 'Không có số điện thoại',
          'fullName': result['fullName']?.toString() ?? 'Unknown',
          'gender': result['gender']?.toString() ?? 'Không xác định',
          'dateOfBirth': result['dateOfBirth']?.toString() ?? '',
          'branchesImage': result['branchesImage']?.startsWith('http') == true
              ? result['branchesImage']
              : 'https://via.placeholder.com/120',
        };
        _isLoading = false;
      });
    } catch (e) {
      print('Error fetching employee data: $e');
      setState(() {
        _error = e.toString();
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        backgroundColor: primaryColor,
        title: Text(
          'Thông tin nhân viên',
          style: TextStyle(color: textColor, fontFamily: 'Poppins', fontWeight: FontWeight.w600),
        ),
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: _isLoading
          ? Center(
              child: CircularProgressIndicator(
                valueColor: AlwaysStoppedAnimation<Color>(accentColor),
              ),
            )
          : _error != null
              ? Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(
                        'Lỗi: $_error',
                        style: TextStyle(color: Colors.redAccent, fontSize: 16, fontFamily: 'Poppins'),
                        textAlign: TextAlign.center,
                      ),
                      SizedBox(height: 16),
                      ElevatedButton(
                        onPressed: _fetchEmployeeDetails,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: accentColor,
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                        ),
                        child: Text(
                          'Thử lại',
                          style: TextStyle(color: textColor, fontFamily: 'Poppins'),
                        ),
                      ),
                    ],
                  ),
                )
              : _employee == null || _employee!.isEmpty
                  ? Center(
                      child: Text(
                        'Dữ liệu nhân viên trống',
                        style: TextStyle(color: Colors.redAccent, fontSize: 16, fontFamily: 'Poppins'),
                      ),
                    )
                  : SingleChildScrollView(
                      padding: EdgeInsets.all(16),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: [
                          // Profile Header
                          CircleAvatar(
                            radius: 60,
                            backgroundImage: NetworkImage(_employee!['image']),
                            backgroundColor: primaryColor,
                            onBackgroundImageError: (_, __) => Icon(Icons.broken_image, color: Colors.redAccent),
                          ),
                          SizedBox(height: 16),
                          Text(
                            _employee!['fullName'],
                            style: TextStyle(
                              fontSize: 24,
                              fontWeight: FontWeight.bold,
                              color: textColor,
                              fontFamily: 'Poppins',
                            ),
                            textAlign: TextAlign.center,
                          ),
                          SizedBox(height: 8),
                          Text(
                            _employee!['positionName'],
                            style: TextStyle(
                              fontSize: 16,
                              color: accentColor,
                              fontFamily: 'Poppins',
                            ),
                          ),
                          SizedBox(height: 8),
                          Text(
                            _employee!['specialtyName'],
                            style: TextStyle(
                              fontSize: 14,
                              color: textColor.withOpacity(0.8),
                              fontFamily: 'Poppins',
                            ),
                          ),
                          SizedBox(height: 16),

                          // Personal Information Card
                          Card(
                            elevation: 5,
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                            color: primaryColor.withOpacity(0.8),
                            child: Padding(
                              padding: EdgeInsets.all(16),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    'Thông tin cá nhân',
                                    style: TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.bold,
                                      color: textColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                  SizedBox(height: 8),
                                  _buildInfoRow('Email', _employee!['email']),
                                  _buildInfoRow('Số điện thoại', _employee!['phone']),
                                  _buildInfoRow('Giới tính', _employee!['gender']),
                                  _buildInfoRow(
                                    'Ngày sinh',
                                    _employee!['dateOfBirth'].isNotEmpty
                                        ? DateFormat('dd/MM/yyyy').format(DateTime.parse(_employee!['dateOfBirth']))
                                        : 'Không có thông tin',
                                  ),
                                ],
                              ),
                            ),
                          ),
                          SizedBox(height: 16),

                          // Work Information Card
                          Card(
                            elevation: 5,
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                            color: primaryColor.withOpacity(0.8),
                            child: Padding(
                              padding: EdgeInsets.all(16),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    'Thông tin công việc',
                                    style: TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.bold,
                                      color: textColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                  SizedBox(height: 8),
                                  _buildInfoRow('Mã nhân viên', _employee!['empID']),
                                  _buildInfoRow('Mã định danh', _employee!['empCode']),
                                  _buildInfoRow('Mã vị trí', _employee!['positionID']),
                                  _buildInfoRow('Vị trí', _employee!['positionName']),
                                  _buildInfoRow('Mã chuyên môn', _employee!['specialtyID']),
                                  _buildInfoRow('Chuyên môn', _employee!['specialtyName']),
                                  _buildInfoRow(
                                    'Lương cơ bản',
                                    NumberFormat.currency(locale: 'vi_VN', symbol: 'VNĐ').format(int.parse(_employee!['salary'])),
                                  ),
                                  _buildInfoRow(
                                    'Thưởng',
                                    NumberFormat.currency(locale: 'vi_VN', symbol: 'VNĐ').format(int.parse(_employee!['bonusSalary'])),
                                  ),
                                  _buildInfoRow(
                                    'Ngày bắt đầu',
                                    _employee!['startDate'].isNotEmpty
                                        ? DateFormat('dd/MM/yyyy').format(DateTime.parse(_employee!['startDate']))
                                        : 'Không có thông tin',
                                  ),
                                  _buildInfoRow('Trạng thái', _employee!['status']),
                                  _buildInfoRow('Mã chi nhánh', _employee!['branchID']),
                                  _buildInfoRow('Chi nhánh', _employee!['branchName']),
                                ],
                              ),
                            ),
                          ),
                          SizedBox(height: 16),

                          // Branch Image Card
                          Card(
                            elevation: 5,
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                            color: primaryColor.withOpacity(0.8),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Padding(
                                  padding: EdgeInsets.all(16),
                                  child: Text(
                                    'Hình ảnh chi nhánh',
                                    style: TextStyle(
                                      fontSize: 18,
                                      fontWeight: FontWeight.bold,
                                      color: textColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                ),
                                ClipRRect(
                                  borderRadius: BorderRadius.only(
                                    bottomLeft: Radius.circular(12),
                                    bottomRight: Radius.circular(12),
                                  ),
                                  child: Image.network(
                                    _employee!['branchesImage'],
                                    height: 150,
                                    width: double.infinity,
                                    fit: BoxFit.cover,
                                    errorBuilder: (context, error, stackTrace) => Container(
                                      height: 150,
                                      color: primaryColor,
                                      child: Icon(
                                        Icons.broken_image,
                                        color: Colors.redAccent,
                                        size: 50,
                                      ),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                    ),
    );
  }

  Widget _buildInfoRow(String label, String value) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 4),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 120,
            child: Text(
              '$label: ',
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.w600,
                color: textColor,
                fontFamily: 'Poppins',
              ),
            ),
          ),
          Expanded(
            child: Text(
              value.length > 50 ? '${value.substring(0, 47)}...' : value,
              style: TextStyle(
                fontSize: 14,
                color: textColor,
                fontFamily: 'Poppins',
              ),
              softWrap: true,
            ),
          ),
        ],
      ),
    );
  }
}