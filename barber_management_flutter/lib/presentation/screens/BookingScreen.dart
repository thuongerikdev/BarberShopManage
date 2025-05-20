import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:barbermanagemobile/domain/entities/promotion.dart';
import 'package:barbermanagemobile/domain/entities/booking_create_order.dart';
import 'package:barbermanagemobile/domain/usecases/create_booking_order_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_all_branches_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_booking_service_detail_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_branch_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_employees_by_date_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_promotions_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_promotions_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/create_customer_promotion_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_service_by_id_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:get_it/get_it.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class BookingScreen extends StatefulWidget {
  final VoidCallback onBack;

  const BookingScreen({super.key, required this.onBack});

  @override
  _BookingScreenState createState() => _BookingScreenState();
}

class _BookingScreenState extends State<BookingScreen> {
  final GetAllBranchesUseCase _getAllBranchesUseCase =
      GetIt.instance<GetAllBranchesUseCase>();
  final GetEmployeesByBranchUseCase _getEmployeesByBranchUseCase =
      GetIt.instance<GetEmployeesByBranchUseCase>();
  final GetEmployeesByDateUseCase _getEmployeesByDateUseCase =
      GetIt.instance<GetEmployeesByDateUseCase>();
  final GetBookingServiceDetailUseCase _getBookingServiceDetailUseCase =
      GetIt.instance<GetBookingServiceDetailUseCase>();
  final CreateBookingOrderUseCase _createBookingOrderUseCase =
      GetIt.instance<CreateBookingOrderUseCase>();
  final GetCustomerByUserIDUseCase _getCustomerByUserIDUseCase =
      GetIt.instance<GetCustomerByUserIDUseCase>();
  final GetPromotionsUseCase _getPromotionsUseCase =
      GetIt.instance<GetPromotionsUseCase>();
  final GetCustomerPromotionsUseCase _getCustomerPromotionsUseCase =
      GetIt.instance<GetCustomerPromotionsUseCase>();
  final CreateCustomerPromotionUseCase _createCustomerPromotionUseCase =
      GetIt.instance<CreateCustomerPromotionUseCase>();
  final GetServiceByIdUseCase _getServiceByIdUseCase =
      GetIt.instance<GetServiceByIdUseCase>();

  List<Map<String, dynamic>> branches = [];
  List<Map<String, dynamic>> employees = [];
  List<Map<String, dynamic>> availableEmployeesByDate = [];
  List<Map<String, dynamic>> serviceDetails = [];
  List<Promotion> promotions = [];
  Set<int> customerPromoIds = {};
  Promotion? selectedPromotion;

  // Updated state for multiple appointments
  List<String> selectedServiceDetails = [];
  Map<String, String> serviceEmployeeMap = {}; // Maps serviceDetailID to empID
  DateTime? selectedDateTime;
  String? selectedBranch;
  String? selectedTypeOfEmp;
  int? custID;
  bool isLoading = false;
  String? customerErrorMessage;
  String? promotionErrorMessage;
  bool hasNoPromotions = false;

  int customerPoints = 1000;

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const dropdownTextColor = Color(0xFFD7CCC8);

  final int defaultServiceID = 1;

  @override
  void initState() {
    super.initState();
    _loadCustomerID();
    _loadBranches();
    _loadPromotions();
  }

  Future<void> _loadCustomerID() async {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final user = authProvider.user;
    if (user == null) {
      setState(() {
        customerErrorMessage = 'Vui lòng đăng nhập để đặt lịch';
      });
      if (kDebugMode) {
        print('No user logged in');
      }
      return;
    }

    final userId = user.userId;
    if (userId.isEmpty || int.tryParse(userId) == null) {
      setState(() {
        customerErrorMessage =
            'Thông tin người dùng không hợp lệ (userId không khả dụng)';
      });
      if (kDebugMode) {
        print('Invalid userId: $userId');
      }
      return;
    }

    final userIdInt = int.parse(userId);
    if (kDebugMode) {
      print('Loading customer ID for userId: $userIdInt');
    }

    setState(() => isLoading = true);
    try {
      final result = await _getCustomerByUserIDUseCase.call(userIdInt);
      result.fold(
        (error) => throw Exception(error),
        (customer) => setState(() {
          custID = customer.customerID;
          isLoading = false;
          if (kDebugMode) print('Customer: $customer');
        }),
      );
      await _loadCustomerPromotions();
    } catch (e) {
      setState(() {
        isLoading = false;
        customerErrorMessage = 'Lỗi khi lấy thông tin khách hàng: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi khi lấy thông tin khách hàng: $e'),
            backgroundColor: primaryColor,
          ),
        );
      }
    }
  }

  Future<void> _loadCustomerPromotions() async {
    if (custID == null) {
      setState(() {
        isLoading = false;
        promotionErrorMessage = 'custID is null, cannot load promotions';
      });
      return;
    }
    setState(() => isLoading = true);
    try {
      if (kDebugMode) {
        print('Loading promotions for customer ID: $custID');
      }
      final customerPromotions =
          await _getCustomerPromotionsUseCase.call(custID!);
      setState(() {
        customerPromoIds = customerPromotions.map((p) => p.promoID).toSet();
        hasNoPromotions = customerPromoIds.isEmpty;
        isLoading = false;
      });
      if (kDebugMode) {
        print('Loaded customer promotion IDs: $customerPromoIds');
      }
    } catch (e) {
      setState(() {
        isLoading = false;
        promotionErrorMessage =
            'Lỗi khi lấy danh sách mã giảm giá của khách hàng: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
              content:
                  Text('Lỗi khi lấy danh sách mã giảm giá của khách hàng: $e'),
              backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadBranches() async {
    setState(() => isLoading = true);
    try {
      branches = await _getAllBranchesUseCase.call();
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        customerErrorMessage = 'Lỗi khi lấy danh sách chi nhánh: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
              content: Text('Lỗi khi lấy danh sách chi nhánh: $e'),
              backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadEmployeesByBranch(int branchID) async {
    setState(() => isLoading = true);
    try {
      employees = await _getEmployeesByBranchUseCase.call(branchID);
      if (kDebugMode) {
        print('Loaded employees for branch $branchID: $employees');
      }
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        customerErrorMessage = 'Lỗi khi lấy danh sách nhân viên: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
              content: Text('Lỗi khi lấy danh sách nhân viên: $e'),
              backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadEmployeesByDate(
      DateTime date, int branchID, String typeOfEmp) async {
    setState(() => isLoading = true);
    try {
      availableEmployeesByDate =
          await _getEmployeesByDateUseCase.call(date, branchID, typeOfEmp);
      if (kDebugMode) {
        print(
            'Loaded employees by date $date, branch $branchID, typeOfEmp $typeOfEmp: $availableEmployeesByDate');
      }
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        customerErrorMessage = 'Lỗi khi lấy nhân viên theo ngày: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
              content: Text('Lỗi khi lấy nhân viên theo ngày: $e'),
              backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadServiceDetails() async {
    setState(() => isLoading = true);
    try {
      serviceDetails =
          await _getBookingServiceDetailUseCase.call(defaultServiceID);
      if (kDebugMode) {
        print(
            'Loaded service details for service $defaultServiceID: $serviceDetails');
      }
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        customerErrorMessage = 'Lỗi khi lấy chi tiết dịch vụ: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
              content: Text('Lỗi khi lấy chi tiết dịch vụ: $e'),
              backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _loadPromotions() async {
    setState(() => isLoading = true);
    try {
      final fetchedPromotions = await _getPromotionsUseCase.call();
      promotions = [
        Promotion(
          promoID: 0,
          promoName: 'Không áp dụng',
          promoDescription: '',
          promoDiscount: 0.0,
          pointToGet: 0,
          promoImage: '',
          promoStart: DateTime.now(),
          promoEnd: DateTime.now(),
          promoStatus: 'OK',
          promoType: 'none',
          authCusPromos: null,
        ),
        ...fetchedPromotions,
      ];
      if (kDebugMode) {
        print(
            'Loaded promotions: ${promotions.map((p) => p.promoName).toList()}');
      }
      setState(() => isLoading = false);
    } catch (e) {
      setState(() {
        isLoading = false;
        promotionErrorMessage = 'Lỗi khi lấy danh sách khuyến mãi: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
              content: Text('Lỗi khi lấy danh sách khuyến mãi: $e'),
              backgroundColor: primaryColor),
        );
      }
    }
  }

  Future<void> _claimPromotion(
      int promoId, String promoName, String promoType, int pointToGet) async {
    if (custID == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Vui lòng đăng nhập để lấy mã giảm giá'),
          backgroundColor: primaryColor,
          behavior: SnackBarBehavior.floating,
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        ),
      );
      return;
    }

    if (promoType == 'TransferPromotion' && customerPoints < pointToGet) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(
              'Không đủ điểm: Cần $pointToGet điểm, bạn có $customerPoints điểm'),
          backgroundColor: Colors.redAccent,
          behavior: SnackBarBehavior.floating,
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        ),
      );
      return;
    }

    setState(() => isLoading = true);
    try {
      await _createCustomerPromotionUseCase.call(custID!, promoId, 'Active');
      await _loadCustomerPromotions();
      await _loadPromotions();
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Đã lấy mã: $promoName',
                style: TextStyle(color: textColor)),
            backgroundColor: primaryColor,
            behavior: SnackBarBehavior.floating,
            shape:
                RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi lấy mã: $e', style: TextStyle(color: textColor)),
            backgroundColor: Colors.redAccent,
            behavior: SnackBarBehavior.floating,
            shape:
                RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          ),
        );
      }
    } finally {
      setState(() => isLoading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: primaryColor,
        title: Text(
          "Đặt lịch giữ chỗ",
          style: TextStyle(
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
            fontSize: 24,
            color: textColor,
          ),
        ),
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: widget.onBack,
        ),
      ),
      body: isLoading
          ? Center(child: CircularProgressIndicator(color: accentColor))
          : SingleChildScrollView(
              padding: EdgeInsets.symmetric(horizontal: 16, vertical: 20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _buildStep(
                    stepNumber: 1,
                    title: "Chọn salon",
                    isActive: true,
                    child: _buildSelectionButton(
                      icon: Icons.store,
                      text: selectedBranch == null
                          ? "Xem tất cả salon"
                          : branches.firstWhere((b) =>
                              b['branchID'].toString() ==
                              selectedBranch)['branchName'],
                      imageUrl: selectedBranch != null
                          ? branches.firstWhere((b) =>
                              b['branchID'].toString() ==
                              selectedBranch)['branchImage']
                          : null,
                      onTap: () async {
                        await _loadBranches();
                        if (mounted) {
                          showModalBottomSheet(
                            context: context,
                            builder: (context) => Container(
                              color: primaryColor.withOpacity(0.9),
                              child: ListView(
                                children: branches.map((branch) {
                                  return ListTile(
                                    leading: branch['branchImage'].isNotEmpty
                                        ? ClipRRect(
                                            borderRadius:
                                                BorderRadius.circular(8),
                                            child: Image.network(
                                              branch['branchImage'],
                                              width: 40,
                                              height: 40,
                                              fit: BoxFit.cover,
                                              errorBuilder:
                                                  (context, error, stackTrace) {
                                                if (kDebugMode) {
                                                  print(
                                                      'Error loading branch image: $error');
                                                }
                                                return Icon(
                                                  Icons.store,
                                                  color: textColor,
                                                  size: 40,
                                                );
                                              },
                                            ),
                                          )
                                        : Icon(Icons.store, color: textColor),
                                    title: Text(
                                      branch['branchName'],
                                      style:
                                          TextStyle(color: dropdownTextColor),
                                    ),
                                    subtitle: Text(
                                      branch['location'],
                                      style: TextStyle(
                                          color: dropdownTextColor
                                              .withOpacity(0.7)),
                                    ),
                                    onTap: () {
                                      setState(() {
                                        selectedBranch =
                                            branch['branchID'].toString();
                                        selectedServiceDetails.clear();
                                        serviceEmployeeMap.clear();
                                        selectedDateTime = null;
                                        selectedPromotion = null;
                                        selectedTypeOfEmp = null;
                                        _loadEmployeesByBranch(
                                            int.parse(selectedBranch!));
                                      });
                                      Navigator.pop(context);
                                    },
                                  );
                                }).toList(),
                              ),
                            ),
                          );
                        }
                      },
                      highlightText:
                          selectedBranch != null ? "Tìm salon gần anh" : null,
                      highlightColor: accentColor,
                    ),
                  ),
                  _buildStep(
                    stepNumber: 2,
                    title: "Chọn dịch vụ chi tiết",
                    isActive: selectedBranch != null,
                    child: _buildSelectionButton(
                      icon: Icons.cut,
                      text: selectedServiceDetails.isEmpty
                          ? "Xem tất cả dịch vụ chi tiết"
                          : "${selectedServiceDetails.length} dịch vụ đã chọn",
                      imageUrl: selectedServiceDetails.isNotEmpty
                          ? serviceDetails.firstWhere((s) =>
                              s['serviceDetailID'].toString() ==
                              selectedServiceDetails.first)['servImage']
                          : null,
                      onTap: selectedBranch != null
                          ? () async {
                              await _loadServiceDetails();
                              if (mounted) {
                                showModalBottomSheet(
                                  context: context,
                                  builder: (context) => Container(
                                    color: primaryColor.withOpacity(0.9),
                                    child: StatefulBuilder(
                                      builder: (context, setModalState) {
                                        return Column(
                                          children: [
                                            Padding(
                                              padding: const EdgeInsets.all(16),
                                              child: Text(
                                                'Chọn nhiều dịch vụ',
                                                style: TextStyle(
                                                  color: textColor,
                                                  fontSize: 18,
                                                  fontWeight: FontWeight.w600,
                                                ),
                                              ),
                                            ),
                                            Expanded(
                                              child: ListView(
                                                children: serviceDetails
                                                    .map((serviceDetail) {
                                                  final isSelected =
                                                      selectedServiceDetails
                                                          .contains(serviceDetail[
                                                                  'serviceDetailID']
                                                              .toString());
                                                  return CheckboxListTile(
                                                    activeColor: accentColor,
                                                    checkColor: textColor,
                                                    title: Text(
                                                      "${serviceDetail['servName']} - ${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(serviceDetail['servPrice'])}",
                                                      style: TextStyle(
                                                          color:
                                                              dropdownTextColor),
                                                    ),
                                                    subtitle: Text(
                                                      serviceDetail[
                                                          'servDescription'],
                                                      style: TextStyle(
                                                          color:
                                                              dropdownTextColor
                                                                  .withOpacity(
                                                                      0.7)),
                                                      maxLines: 2,
                                                      overflow:
                                                          TextOverflow.ellipsis,
                                                    ),
                                                    value: isSelected,
                                                    onChanged: (value) async {
                                                      setModalState(() {
                                                        if (value == true) {
                                                          selectedServiceDetails
                                                              .add(serviceDetail[
                                                                      'serviceDetailID']
                                                                  .toString());
                                                        } else {
                                                          selectedServiceDetails
                                                              .remove(serviceDetail[
                                                                      'serviceDetailID']
                                                                  .toString());
                                                          serviceEmployeeMap.remove(
                                                              serviceDetail[
                                                                      'serviceDetailID']
                                                                  .toString());
                                                        }
                                                      });
                                                      setState(() {});
                                                      if (value == true) {
                                                        try {
                                                          final serviceIdDynamic =
                                                              serviceDetail[
                                                                  'servID'];
                                                          if (serviceIdDynamic ==
                                                              null) {
                                                            throw Exception(
                                                                'serviceID is missing in serviceDetail');
                                                          }
                                                          final serviceId = serviceIdDynamic
                                                                  is String
                                                              ? int.parse(
                                                                  serviceIdDynamic)
                                                              : serviceIdDynamic
                                                                      is int
                                                                  ? serviceIdDynamic
                                                                  : throw Exception(
                                                                      'serviceID is not a valid integer');
                                                          final serviceData =
                                                              await _getServiceByIdUseCase
                                                                  .call(
                                                                      serviceId);
                                                          final servName = serviceData[
                                                                      'servName']
                                                                  ?.toString() ??
                                                              '';
                                                          setState(() {
                                                            selectedTypeOfEmp =
                                                                servName.toLowerCase() ==
                                                                        'massage'
                                                                    ? 'Massage'
                                                                    : 'HairCutting';
                                                          });
                                                          if (kDebugMode) {
                                                            print(
                                                                'Selected service: $servName, typeOfEmp: $selectedTypeOfEmp');
                                                          }
                                                        } catch (e) {
                                                          setState(() {
                                                            customerErrorMessage =
                                                                'Lỗi khi lấy thông tin dịch vụ: $e';
                                                          });
                                                          ScaffoldMessenger.of(
                                                                  context)
                                                              .showSnackBar(
                                                            SnackBar(
                                                                content: Text(
                                                                    'Lỗi khi lấy thông tin dịch vụ: $e'),
                                                                backgroundColor:
                                                                    primaryColor),
                                                          );
                                                        }
                                                      }
                                                    },
                                                    secondary: serviceDetail[
                                                                'servImage']
                                                            .isNotEmpty
                                                        ? ClipRRect(
                                                            borderRadius:
                                                                BorderRadius
                                                                    .circular(
                                                                        8),
                                                            child:
                                                                Image.network(
                                                              serviceDetail[
                                                                  'servImage'],
                                                              width: 40,
                                                              height: 40,
                                                              fit: BoxFit.cover,
                                                              errorBuilder:
                                                                  (context,
                                                                      error,
                                                                      stackTrace) {
                                                                if (kDebugMode) {
                                                                  print(
                                                                      'Error loading service image: $error');
                                                                }
                                                                return Icon(
                                                                  Icons.cut,
                                                                  color:
                                                                      textColor,
                                                                  size: 40,
                                                                );
                                                              },
                                                            ),
                                                          )
                                                        : Icon(Icons.cut,
                                                            color: textColor),
                                                  );
                                                }).toList(),
                                              ),
                                            ),
                                            Padding(
                                              padding:
                                                  const EdgeInsets.all(16.0),
                                              child: ElevatedButton(
                                                onPressed: () {
                                                  Navigator.pop(context);
                                                },
                                                style: ElevatedButton.styleFrom(
                                                  backgroundColor: accentColor,
                                                  padding: EdgeInsets.symmetric(
                                                      vertical: 12),
                                                  shape: RoundedRectangleBorder(
                                                      borderRadius:
                                                          BorderRadius.circular(
                                                              8)),
                                                  elevation: 0,
                                                ),
                                                child: Text(
                                                  'Xong',
                                                  style: TextStyle(
                                                    fontFamily: 'Poppins',
                                                    fontSize: 16,
                                                    fontWeight: FontWeight.w600,
                                                    color: textColor,
                                                  ),
                                                ),
                                              ),
                                            ),
                                          ],
                                        );
                                      },
                                    ),
                                  ),
                                );
                              }
                            }
                          : null,
                    ),
                  ),
                  _buildStep(
                    stepNumber: 3,
                    title: "Chọn ngày, giờ & stylist",
                    isActive: selectedServiceDetails.isNotEmpty,
                    child: Column(
                      children: [
                        _buildSelectionButton(
                          icon: Icons.calendar_today,
                          text: selectedDateTime == null
                              ? "Hôm nay, ${DateFormat('EEE (dd/MM)').format(DateTime.now())}"
                              : _getDateDisplayText(selectedDateTime!),
                          onTap: selectedServiceDetails.isNotEmpty
                              ? () async {
                                  final date = await showDatePicker(
                                    context: context,
                                    initialDate: DateTime.now(),
                                    firstDate: DateTime.now(),
                                    lastDate:
                                        DateTime.now().add(Duration(days: 30)),
                                    builder: (context, child) {
                                      return Theme(
                                        data: ThemeData.dark().copyWith(
                                          colorScheme: ColorScheme.dark(
                                            primary: accentColor,
                                            onPrimary: textColor,
                                            surface:
                                                primaryColor.withOpacity(0.9),
                                            onSurface: dropdownTextColor,
                                          ),
                                        ),
                                        child: child!,
                                      );
                                    },
                                  );
                                  if (date != null) {
                                    final time = await showTimePicker(
                                        context: context,
                                        initialTime: TimeOfDay.now(),
                                        builder: (context, child) {
                                          return Theme(
                                            data: ThemeData.dark().copyWith(
                                              colorScheme: ColorScheme.dark(
                                                primary: accentColor,
                                                onPrimary: textColor,
                                                surface: primaryColor
                                                    .withOpacity(0.9),
                                                onSurface: dropdownTextColor,
                                              ),
                                            ),
                                            child: child!,
                                          );
                                        });
                                    if (time != null && mounted) {
                                      setState(() {
                                        selectedDateTime = DateTime(
                                            date.year,
                                            date.month,
                                            date.day,
                                            time.hour,
                                            time.minute);
                                      });
                                      if (selectedBranch != null &&
                                          selectedTypeOfEmp != null) {
                                        await _loadEmployeesByDate(
                                            selectedDateTime!,
                                            int.parse(selectedBranch!),
                                            selectedTypeOfEmp!);
                                      }
                                    }
                                  }
                                }
                              : null,
                          highlightText:
                              selectedDateTime != null ? "Ngày thường" : null,
                          highlightColor: Colors.green,
                        ),
                        if (selectedDateTime != null &&
                            availableEmployeesByDate.isNotEmpty)
                          ...selectedServiceDetails.map((serviceDetailID) {
                            final service = serviceDetails.firstWhere((s) =>
                                s['serviceDetailID'].toString() ==
                                serviceDetailID);
                            final empID = serviceEmployeeMap[serviceDetailID];
                            return Padding(
                              padding: const EdgeInsets.only(top: 8),
                              child: _buildSelectionButton(
                                icon: Icons.person,
                                text: empID == null
                                    ? "Chọn stylist cho ${service['servName']}"
                                    : availableEmployeesByDate.firstWhere(
                                        (e) => e['empID'] == empID)['empName'],
                                imageUrl: empID != null
                                    ? availableEmployeesByDate.firstWhere(
                                        (e) => e['empID'] == empID)['image']
                                    : null,
                                onTap: () async {
                                  showModalBottomSheet(
                                    context: context,
                                    builder: (context) => Container(
                                      color: primaryColor.withOpacity(0.9),
                                      child: ListView(
                                        children: availableEmployeesByDate
                                            .map((employee) {
                                          return ListTile(
                                            leading: employee['image']
                                                    .isNotEmpty
                                                ? ClipRRect(
                                                    borderRadius:
                                                        BorderRadius.circular(
                                                            8),
                                                    child: Image.network(
                                                      employee['image'],
                                                      width: 40,
                                                      height: 40,
                                                      fit: BoxFit.cover,
                                                      errorBuilder: (context,
                                                          error, stackTrace) {
                                                        if (kDebugMode) {
                                                          print(
                                                              'Error loading employee image: $error');
                                                        }
                                                        return Icon(
                                                          Icons.person,
                                                          color: textColor,
                                                          size: 40,
                                                        );
                                                      },
                                                    ),
                                                  )
                                                : Icon(Icons.person,
                                                    color: textColor),
                                            title: Text(
                                              "${employee['empName']} (${employee['position']})",
                                              style: TextStyle(
                                                  color: dropdownTextColor),
                                            ),
                                            subtitle: Text(
                                              "Ca: ${employee['startTime']} - ${employee['endTime']}",
                                              style: TextStyle(
                                                  color: dropdownTextColor
                                                      .withOpacity(0.7)),
                                            ),
                                            onTap: () {
                                              setState(() {
                                                serviceEmployeeMap[
                                                        serviceDetailID] =
                                                    employee['empID'];
                                              });
                                              Navigator.pop(context);
                                            },
                                          );
                                        }).toList(),
                                      ),
                                    ),
                                  );
                                },
                              ),
                            );
                          }).toList(),
                      ],
                    ),
                  ),
                  _buildStep(
                    stepNumber: 4,
                    title: "Chọn mã giảm giá",
                    isActive: serviceEmployeeMap.length ==
                        selectedServiceDetails.length,
                    child: _buildSelectionButton(
                      icon: Icons.local_offer,
                      text: selectedPromotion == null
                          ? "Chọn mã giảm giá"
                          : "${selectedPromotion!.promoName} (${selectedPromotion!.promoDiscount.toInt()}%)",
                      imageUrl: selectedPromotion?.promoImage,
                      onTap: serviceEmployeeMap.length ==
                              selectedServiceDetails.length
                          ? () async {
                              showModalBottomSheet(
                                context: context,
                                builder: (context) => Container(
                                  color: primaryColor.withOpacity(0.9),
                                  child: ListView(
                                    children: promotions.map((promo) {
                                      final isCustomerOwned = customerPromoIds
                                              .contains(promo.promoID) ||
                                          promo.promoID == 0;
                                      return ListTile(
                                        leading: promo.promoImage.isNotEmpty
                                            ? ClipRRect(
                                                borderRadius:
                                                    BorderRadius.circular(8),
                                                child: Image.network(
                                                  promo.promoImage,
                                                  width: 40,
                                                  height: 40,
                                                  fit: BoxFit.cover,
                                                  errorBuilder: (context, error,
                                                      stackTrace) {
                                                    if (kDebugMode) {
                                                      print(
                                                          'Error loading promotion image: $error');
                                                    }
                                                    return Icon(
                                                      Icons.local_offer,
                                                      color: textColor,
                                                      size: 40,
                                                    );
                                                  },
                                                ),
                                              )
                                            : Icon(Icons.local_offer,
                                                color: textColor, size: 40),
                                        title: Text(
                                          "${promo.promoName} (${promo.promoDiscount.toInt()}%)",
                                          style: TextStyle(
                                            color: isCustomerOwned
                                                ? dropdownTextColor
                                                : dropdownTextColor
                                                    .withOpacity(0.5),
                                          ),
                                        ),
                                        subtitle: Column(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                              promo.promoDescription,
                                              style: TextStyle(
                                                color: isCustomerOwned
                                                    ? dropdownTextColor
                                                        .withOpacity(0.7)
                                                    : dropdownTextColor
                                                        .withOpacity(0.3),
                                              ),
                                            ),
                                            if (promo.promoType ==
                                                'TransferPromotion')
                                              Text(
                                                'Điểm cần: ${promo.pointToGet}',
                                                style: TextStyle(
                                                  color: isCustomerOwned
                                                      ? dropdownTextColor
                                                          .withOpacity(0.7)
                                                      : dropdownTextColor
                                                          .withOpacity(0.3),
                                                ),
                                              ),
                                          ],
                                        ),
                                        trailing: !isCustomerOwned &&
                                                (promo.promoType ==
                                                        'FreePromotion' ||
                                                    promo.promoType ==
                                                        'TransferPromotion')
                                            ? ElevatedButton(
                                                onPressed: () async {
                                                  Navigator.pop(context);
                                                  await _claimPromotion(
                                                      promo.promoID,
                                                      promo.promoName,
                                                      promo.promoType,
                                                      promo.pointToGet);
                                                },
                                                style: ElevatedButton.styleFrom(
                                                  backgroundColor: accentColor,
                                                  padding: EdgeInsets.symmetric(
                                                      horizontal: 12,
                                                      vertical: 8),
                                                  shape: RoundedRectangleBorder(
                                                      borderRadius:
                                                          BorderRadius.circular(
                                                              8)),
                                                  elevation: 0,
                                                ),
                                                child: Text(
                                                  'Lấy mã',
                                                  style: TextStyle(
                                                    fontSize: 12,
                                                    fontWeight: FontWeight.w600,
                                                    color: textColor,
                                                    fontFamily: 'Poppins',
                                                  ),
                                                ),
                                              )
                                            : null,
                                        onTap: isCustomerOwned
                                            ? () {
                                                setState(() {
                                                  selectedPromotion = promo;
                                                });
                                                Navigator.pop(context);
                                              }
                                            : null,
                                      );
                                    }).toList(),
                                  ),
                                ),
                              );
                            }
                          : null,
                    ),
                  ),
                  if (_isFormValid())
                    Padding(
                      padding: const EdgeInsets.symmetric(vertical: 16),
                      child: Container(
                        padding: EdgeInsets.all(12),
                        decoration: BoxDecoration(
                          color: primaryColor.withOpacity(0.2),
                          borderRadius: BorderRadius.circular(8),
                        ),
                        child: Text(
                          "Cắt xong trả tiền, hủy lịch không sao",
                          style: TextStyle(color: textColor, fontSize: 14),
                        ),
                      ),
                    ),
                  if (_isFormValid())
                    SizedBox(
                      width: double.infinity,
                      child: ElevatedButton(
                        onPressed: () => _showConfirmationDialog(context),
                        style: ElevatedButton.styleFrom(
                          backgroundColor: accentColor,
                          padding: EdgeInsets.symmetric(vertical: 16),
                          shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(8)),
                          elevation: 0,
                        ),
                        child: Text(
                          "Chốt giờ cắt",
                          style: TextStyle(
                            fontFamily: 'Poppins',
                            fontSize: 16,
                            fontWeight: FontWeight.w600,
                            color: textColor,
                          ),
                        ),
                      ),
                    ),
                  if (customerErrorMessage != null)
                    Padding(
                      padding: const EdgeInsets.only(top: 16),
                      child: Text(
                        customerErrorMessage!,
                        style: TextStyle(color: Colors.red, fontSize: 14),
                      ),
                    ),
                  if (promotionErrorMessage != null)
                    Padding(
                      padding: const EdgeInsets.only(top: 8),
                      child: Text(
                        promotionErrorMessage!,
                        style: TextStyle(color: Colors.red, fontSize: 14),
                      ),
                    ),
                  if (hasNoPromotions && promotionErrorMessage == null)
                    Padding(
                      padding: const EdgeInsets.only(top: 8),
                      child: Text(
                        'Bạn không có mã giảm giá',
                        style: TextStyle(color: Colors.grey, fontSize: 14),
                      ),
                    ),
                ],
              ),
            ),
    );
  }

  String _getDateDisplayText(DateTime dateTime) {
    final now = DateTime.now();
    final today = DateTime(now.year, now.month, now.day);
    final selectedDate = DateTime(dateTime.year, dateTime.month, dateTime.day);
    final formatter = DateFormat('EEE (dd/MM)');

    if (selectedDate == today) {
      return "Hôm nay, ${formatter.format(dateTime)}";
    } else if (selectedDate == today.add(Duration(days: 1))) {
      return "Ngày mai, ${formatter.format(dateTime)}";
    } else if (selectedDate == today.add(Duration(days: 2))) {
      return "Ngày kia, ${formatter.format(dateTime)}";
    } else {
      return formatter.format(dateTime);
    }
  }

  bool _isFormValid() {
    return selectedBranch != null &&
        selectedDateTime != null &&
        selectedServiceDetails.isNotEmpty &&
        serviceEmployeeMap.length == selectedServiceDetails.length &&
        custID != null &&
        selectedTypeOfEmp != null;
  }

  void _showConfirmationDialog(BuildContext context) {
    final selectedBranchData =
        branches.firstWhere((b) => b['branchID'].toString() == selectedBranch);

    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        backgroundColor: backgroundColor,
        title: Text(
          "Xác Nhận Đặt Lịch",
          style: TextStyle(
              fontFamily: 'Poppins',
              fontWeight: FontWeight.w700,
              color: textColor),
        ),
        content: SingleChildScrollView(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _buildConfirmationRow(
                "Cơ sở",
                selectedBranchData['branchName'],
                imageUrl: selectedBranchData['branchImage'],
              ),
              _buildConfirmationRow("Thời gian",
                  DateFormat('dd/MM/yyyy - HH:mm').format(selectedDateTime!)),
              _buildConfirmationRow("Loại nhân viên", selectedTypeOfEmp!),
              ...selectedServiceDetails.map((serviceDetailID) {
                final serviceDetail = serviceDetails.firstWhere(
                    (s) => s['serviceDetailID'].toString() == serviceDetailID);
                final empID = serviceEmployeeMap[serviceDetailID];
                final employee = availableEmployeesByDate
                    .firstWhere((e) => e['empID'] == empID);
                return Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    _buildConfirmationRow(
                      "Dịch vụ chi tiết",
                      serviceDetail['servName'],
                      imageUrl: serviceDetail['servImage'],
                    ),
                    _buildConfirmationRow(
                      "Stylist",
                      employee['empName'],
                      imageUrl: employee['image'],
                    ),
                  ],
                );
              }).toList(),
              if (selectedPromotion != null && selectedPromotion!.promoID != 0)
                _buildConfirmationRow(
                  "Mã giảm giá",
                  "${selectedPromotion!.promoName} (${selectedPromotion!.promoDiscount.toInt()}%)",
                  imageUrl: selectedPromotion!.promoImage,
                ),
            ],
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: Text("Hủy", style: TextStyle(color: Colors.grey)),
          ),
          Builder(
            builder: (dialogContext) => ElevatedButton(
              onPressed: () async {
                try {
                  final bookingOrder = BookingCreateOrder(
                    appoint: selectedServiceDetails.map((serviceDetailID) {
                      return BookingCreateBusinessAppoint(
                        servID: int.parse(serviceDetailID),
                        empID: int.parse(serviceEmployeeMap[serviceDetailID]!),
                        appStatus: "OK",
                      );
                    }).toList(),
                    order: BookingCreateOrderBusiness(
                      custID: custID!,
                      createAt: DateTime.now(),
                      orderDate: selectedDateTime!,
                    ),
                    promoID: selectedPromotion?.promoID ?? 0,
                  );

                  setState(() => isLoading = true);
                  final result =
                      await _createBookingOrderUseCase.call(bookingOrder);
                  result.fold(
                    (error) => throw Exception(error),
                    (_) {
                      setState(() {
                        isLoading = false;
                        customerErrorMessage = null;
                        promotionErrorMessage = null; // Fixed here
                        hasNoPromotions = false;
                      });

                      Navigator.pop(dialogContext);
                      ScaffoldMessenger.of(dialogContext).showSnackBar(
                        SnackBar(
                          content: Text("Đặt lịch thành công!",
                              style: TextStyle(color: textColor)),
                          backgroundColor: primaryColor,
                          behavior: SnackBarBehavior.floating,
                          shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(12)),
                        ),
                      );
                      widget.onBack();
                    },
                  );
                } catch (e) {
                  setState(() {
                    isLoading = false;
                    customerErrorMessage = "Lỗi đặt lịch: $e";
                  });
                  ScaffoldMessenger.of(dialogContext).showSnackBar(
                    SnackBar(
                      content: Text("Lỗi đặt lịch: $e",
                          style: TextStyle(color: textColor)),
                      backgroundColor: Colors.redAccent,
                      behavior: SnackBarBehavior.floating,
                      shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(12)),
                    ),
                  );
                }
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: accentColor,
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(10)),
              ),
              child: Text("Xác nhận", style: TextStyle(color: textColor)),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildStep(
      {required int stepNumber,
      required String title,
      required bool isActive,
      required Widget child}) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Column(
          children: [
            Container(
              width: 24,
              height: 24,
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                color: isActive ? primaryColor : Colors.grey.shade300,
              ),
              child: Center(
                child: Text(
                  stepNumber.toString(),
                  style: TextStyle(
                    color: textColor,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                ),
              ),
            ),
            if (stepNumber < 4)
              Container(
                width: 2,
                height: 60,
                color: isActive ? Colors.grey.shade300 : Colors.grey.shade200,
              ),
          ],
        ),
        SizedBox(width: 16),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                "$stepNumber. $title",
                style: TextStyle(
                  fontSize: 18,
                  fontWeight: FontWeight.w600,
                  color: textColor,
                  fontFamily: 'Poppins',
                ),
              ),
              SizedBox(height: 8),
              child,
            ],
          ),
        ),
      ],
    );
  }

  Widget _buildSelectionButton({
    required IconData icon,
    required String text,
    required Future<void> Function()? onTap,
    String? highlightText,
    Color? highlightColor,
    String? imageUrl,
  }) {
    return GestureDetector(
      onTap: onTap != null ? () async => await onTap() : null,
      child: Container(
        padding: EdgeInsets.symmetric(vertical: 12, horizontal: 16),
        decoration: BoxDecoration(
          color: primaryColor.withOpacity(0.2),
          borderRadius: BorderRadius.circular(8),
        ),
        child: Row(
          children: [
            imageUrl != null && imageUrl.isNotEmpty
                ? ClipRRect(
                    borderRadius: BorderRadius.circular(8),
                    child: Image.network(
                      imageUrl,
                      width: 40,
                      height: 40,
                      fit: BoxFit.cover,
                      errorBuilder: (context, error, stackTrace) {
                        if (kDebugMode) {
                          print(
                              'Error loading image in selection button: $error');
                        }
                        return Icon(
                          icon,
                          color: onTap != null ? textColor : Colors.grey,
                        );
                      },
                    ),
                  )
                : Icon(icon, color: onTap != null ? textColor : Colors.grey),
            SizedBox(width: 12),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    text,
                    style: TextStyle(
                      fontFamily: 'Poppins',
                      color: onTap != null ? dropdownTextColor : Colors.grey,
                      fontSize: 14,
                    ),
                  ),
                  if (highlightText != null)
                    Padding(
                      padding: const EdgeInsets.only(top: 4),
                      child: Container(
                        padding:
                            EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                        decoration: BoxDecoration(
                          color: highlightColor ?? accentColor,
                          borderRadius: BorderRadius.circular(4),
                        ),
                        child: Text(
                          highlightText,
                          style: TextStyle(
                            color: textColor,
                            fontSize: 12,
                          ),
                        ),
                      ),
                    ),
                ],
              ),
            ),
            Icon(Icons.chevron_right,
                color: onTap != null ? textColor : Colors.grey),
          ],
        ),
      ),
    );
  }

  Widget _buildConfirmationRow(String label, String value, {String? imageUrl}) {
    return Padding(
      padding: EdgeInsets.symmetric(vertical: 4),
      child: Row(
        children: [
          Text(
            "$label: ",
            style: TextStyle(fontWeight: FontWeight.w600, color: textColor),
          ),
          if (imageUrl != null && imageUrl.isNotEmpty)
            Padding(
              padding: const EdgeInsets.only(right: 8),
              child: ClipRRect(
                borderRadius: BorderRadius.circular(4),
                child: Image.network(
                  imageUrl,
                  width: 24,
                  height: 24,
                  fit: BoxFit.cover,
                  errorBuilder: (context, error, stackTrace) {
                    if (kDebugMode) {
                      print('Error loading image in confirmation: $error');
                    }
                    return Icon(
                      label == "Cơ sở"
                          ? Icons.store
                          : label == "Dịch vụ chi tiết"
                              ? Icons.cut
                              : label == "Stylist"
                                  ? Icons.person
                                  : Icons.local_offer,
                      color: textColor,
                      size: 24,
                    );
                  },
                ),
              ),
            ),
          Expanded(
            child: Text(
              value,
              style: TextStyle(color: dropdownTextColor),
            ),
          ),
        ],
      ),
    );
  }
}
