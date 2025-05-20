import 'package:barbermanagemobile/presentation/screens/PromotionScreen.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:provider/provider.dart';
import 'package:barbermanagemobile/domain/usecases/get_customer_by_user_id_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/create_check_in_use_case.dart';
import 'package:barbermanagemobile/domain/usecases/get_check_in_history_use_case.dart';
import 'package:barbermanagemobile/presentation/providers/auth_provider.dart';
import 'package:table_calendar/table_calendar.dart';
import 'package:shimmer/shimmer.dart';
import 'package:animate_do/animate_do.dart';

class CheckInScreen extends StatefulWidget {
  const CheckInScreen({super.key});

  @override
  _CheckInScreenState createState() => _CheckInScreenState();
}

class _CheckInScreenState extends State<CheckInScreen> {
  final GetCustomerByUserIDUseCase _getCustomerByUserIDUseCase =
      GetIt.instance<GetCustomerByUserIDUseCase>();
  final CreateCheckInUseCase _createCheckInUseCase =
      GetIt.instance<CreateCheckInUseCase>();
  final GetCheckInHistoryUseCase _getCheckInHistoryUseCase =
      GetIt.instance<GetCheckInHistoryUseCase>();

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const gradientStart = Color(0xFF6D4C41);
  static const gradientEnd = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  bool isLoading = false;
  String? errorMessage;
  Map<String, dynamic>? checkInResult;
  int? customerID;
  double? loyaltyPoints; // Store loyalty points from Customer
  List<DateTime> checkInDates = [];
  bool hasCheckedInToday = false;
  int streakCount = 0;
  static const double maxLoyaltyPoints = 10000000.0; // Same as VipScreen

  @override
  void initState() {
    super.initState();
    _loadCustomerID();
  }

  Future<void> _loadCustomerID() async {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final user = authProvider.user;
    if (user == null || user.userId.isEmpty || int.tryParse(user.userId) == null) {
      setState(() {
        errorMessage = 'Vui lòng đăng nhập để điểm danh';
      });
      if (kDebugMode) {
        print('Invalid user: $user');
      }
      return;
    }

    final userIdInt = int.parse(user.userId);
    setState(() => isLoading = true);
    try {
      final customerResult = await _getCustomerByUserIDUseCase.call(userIdInt);
      customerResult.fold(
        (error) => throw Exception(error),
        (customer) async {
          setState(() {
            customerID = customer.customerID;
            loyaltyPoints = customer.loyaltyPoints?.toDouble() ?? 0.0;
          });
          await _fetchCheckInHistory(customer.customerID);
        },
      );
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy thông tin khách hàng: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi khi lấy thông tin: $e', style: const TextStyle(color: textColor)),
            backgroundColor: Colors.redAccent,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          ),
        );
      }
    }
  }

  Future<void> _fetchCheckInHistory(int customerId) async {
    try {
      final historyResult = await _getCheckInHistoryUseCase.call(customerId);
      historyResult.fold(
        (error) => throw Exception(error),
        (history) {
          setState(() {
            checkInDates = history
                .where((checkIn) => checkIn['checkInDate'] != null)
                .map((checkIn) {
                  try {
                    return DateTime.parse(checkIn['checkInDate']);
                  } catch (e) {
                    if (kDebugMode) print('Invalid date format: ${checkIn['checkInDate']}');
                    return null;
                  }
                })
                .where((date) => date != null)
                .map((date) => DateTime(date!.year, date.month, date.day))
                .toList();
            final today = DateTime.now();
            hasCheckedInToday = checkInDates.any((date) =>
                date.year == today.year &&
                date.month == today.month &&
                date.day == today.day);
            streakCount = _calculateStreak(checkInDates);
            isLoading = false;
          });
        },
      );
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi lấy lịch sử điểm danh: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi khi lấy lịch sử: $e', style: const TextStyle(color: textColor)),
            backgroundColor: Colors.redAccent,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          ),
        );
      }
    }
  }

  int _calculateStreak(List<DateTime> dates) {
    if (dates.isEmpty) return 0;
    dates.sort((a, b) => b.compareTo(a)); // Sort descending
    int streak = 0;
    DateTime? previousDate;
    for (var date in dates) {
      if (previousDate == null) {
        previousDate = date;
        streak = 1;
        continue;
      }
      final diff = previousDate.difference(date).inDays;
      if (diff == 1) {
        streak++;
        previousDate = date;
      } else if (diff > 1) {
        break;
      }
    }
    return streak;
  }

  Future<void> _performCheckIn() async {
    if (customerID == null) {
      setState(() {
        errorMessage = 'Không tìm thấy thông tin khách hàng';
      });
      return;
    }

    setState(() {
      isLoading = true;
      errorMessage = null;
      checkInResult = null;
    });

    try {
      final checkInDate = DateTime.now().toUtc().toIso8601String();
      final result = await _createCheckInUseCase.call(
        customerID: customerID!,
        checkInDate: checkInDate,
        checkInStatus: 'OK',
        checkInType: 'Daily',
      );

      result.fold(
        (error) => throw Exception(error),
        (data) async {
          setState(() {
            checkInResult = data;
            hasCheckedInToday = true;
            final checkInDateTime = DateTime.now();
            checkInDates.add(DateTime(
                checkInDateTime.year, checkInDateTime.month, checkInDateTime.day));
            streakCount = _calculateStreak(checkInDates);
            // Update loyaltyPoints from checkInResult or refetch customer
            loyaltyPoints = (checkInResult!['totalLoyaltyPoints'] ?? loyaltyPoints)?.toDouble();
            isLoading = false;
            if (kDebugMode) {
              print('Check-in result: $checkInResult');
              print('Streak count: $streakCount');
              print('Loyalty points: $loyaltyPoints');
            }
          });
          // Optionally refetch customer to ensure latest loyaltyPoints
          final customerResult = await _getCustomerByUserIDUseCase.call(int.parse(Provider.of<AuthProvider>(context, listen: false).user!.userId));
          customerResult.fold(
            (error) => print('Error refetching customer: $error'),
            (customer) => setState(() {
              loyaltyPoints = customer.loyaltyPoints?.toDouble() ?? loyaltyPoints;
            }),
          );
        },
      );

      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(
              'Điểm danh thành công! +${checkInResult?['pointsAdded'] ?? 'N/A'} điểm',
              style: const TextStyle(color: textColor),
            ),
            backgroundColor: primaryColor,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          ),
        );
      }
    } catch (e) {
      setState(() {
        isLoading = false;
        errorMessage = 'Lỗi khi điểm danh: $e';
      });
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Lỗi khi điểm danh: $e', style: const TextStyle(color: textColor)),
            backgroundColor: Colors.redAccent,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
          ),
        );
      }
    }
  }

  void _navigateToPromotionScreen() {
    if (customerID == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: const Text('Vui lòng đăng nhập để xem khuyến mãi', style: TextStyle(color: textColor)),
          backgroundColor: Colors.redAccent,
          behavior: SnackBarBehavior.floating,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
        ),
      );
      return;
    }
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => const PromotionScreen()),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        elevation: 0,
        backgroundColor: Colors.transparent,
        flexibleSpace: Container(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              colors: [gradientStart, gradientEnd],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
        ),
        title: const Text(
          'Điểm Danh Hàng Ngày',
          style: TextStyle(
            fontFamily: 'Poppins',
            fontWeight: FontWeight.w700,
            fontSize: 26,
            color: textColor,
            shadows: [Shadow(color: Colors.black26, blurRadius: 4, offset: Offset(2, 2))],
          ),
        ),
        leading: IconButton(
          icon: const Icon(Icons.arrow_back, color: textColor, size: 28),
          onPressed: () => Navigator.pop(context),
        ),
        actions: [
          IconButton(
            icon: const Icon(Icons.refresh, color: textColor, size: 28),
            onPressed: customerID != null ? () => _fetchCheckInHistory(customerID!) : null,
            tooltip: 'Làm mới lịch sử',
          ),
        ],
      ),
      body: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 20),
        child: isLoading
            ? Shimmer.fromColors(
                baseColor: Colors.grey[800]!,
                highlightColor: Colors.grey[600]!,
                child: _buildShimmerContent(),
              )
            : SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: [
                    FadeInDown(
                      duration: const Duration(milliseconds: 500),
                      child: const Text(
                        'Điểm danh để nhận điểm thưởng!',
                        style: TextStyle(
                          fontFamily: 'Poppins',
                          fontSize: 22,
                          fontWeight: FontWeight.w600,
                          color: textColor,
                          shadows: [
                            Shadow(color: Colors.black26, blurRadius: 4, offset: Offset(1, 1))
                          ],
                        ),
                        textAlign: TextAlign.center,
                      ),
                    ),
                    const SizedBox(height: 20),
                    FadeIn(
                      duration: const Duration(milliseconds: 600),
                      child: Icon(
                        Icons.check_circle_outline,
                        size: 120,
                        color: accentColor.withOpacity(0.9),
                      ),
                    ),
                    const SizedBox(height: 20),
                    FadeInUp(
                      duration: const Duration(milliseconds: 700),
                      child: Card(
                        elevation: 8,
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                        color: primaryColor.withOpacity(0.3),
                        child: Padding(
                          padding: const EdgeInsets.all(20),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              // Loyalty Points Progress Section
                              Text(
                                'Tiến Độ Điểm Tích Lũy',
                                style: TextStyle(
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold,
                                  color: textColor,
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              const SizedBox(height: 16),
                              Text(
                                'Điểm Tích Lũy: ${loyaltyPoints?.toStringAsFixed(0) ?? 'N/A'}',
                                style: TextStyle(
                                  fontSize: 16,
                                  color: textColor,
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              const SizedBox(height: 8),
                              LinearProgressIndicator(
                                value: (loyaltyPoints ?? 0) / maxLoyaltyPoints,
                                backgroundColor: shadowColor,
                                valueColor: AlwaysStoppedAnimation<Color>(accentColor),
                                minHeight: 10,
                                borderRadius: BorderRadius.circular(5),
                              ),
                              const SizedBox(height: 4),
                              Text(
                                'Đạt ${((loyaltyPoints ?? 0) / maxLoyaltyPoints * 100).clamp(0, 100).toStringAsFixed(0)}% trong tổng ${maxLoyaltyPoints.toStringAsFixed(0)} điểm',
                                style: TextStyle(
                                  fontSize: 12,
                                  color: textColor.withOpacity(0.7),
                                  fontFamily: 'Poppins',
                                ),
                              ),
                              const SizedBox(height: 24),
                              // Check-in Result
                              if (checkInResult != null) ...[
                                FadeIn(
                                  duration: const Duration(milliseconds: 800),
                                  child: Column(
                                    children: [
                                      const Text(
                                        'Điểm danh thành công!',
                                        style: TextStyle(
                                          fontFamily: 'Poppins',
                                          fontSize: 20,
                                          fontWeight: FontWeight.w700,
                                          color: textColor,
                                        ),
                                      ),
                                      const SizedBox(height: 12),
                                      Text(
                                        'Điểm nhận được: +${checkInResult!['pointsAdded'] ?? 'N/A'}',
                                        style: const TextStyle(
                                          fontFamily: 'Poppins',
                                          fontSize: 16,
                                          fontWeight: FontWeight.w500,
                                          color: accentColor,
                                        ),
                                      ),
                                      const SizedBox(height: 12),
                                      Text(
                                        'Chuỗi điểm danh: $streakCount ngày',
                                        style: TextStyle(
                                          fontFamily: 'Poppins',
                                          fontSize: 16,
                                          fontWeight: FontWeight.w600,
                                          color: streakCount > 1 ? Colors.amber : textColor,
                                        ),
                                      ),
                                    ],
                                  ),
                                ),
                                const SizedBox(height: 20),
                              ],
                              if (errorMessage != null)
                                FadeIn(
                                  duration: const Duration(milliseconds: 800),
                                  child: Padding(
                                    padding: const EdgeInsets.only(bottom: 20),
                                    child: Text(
                                      errorMessage!,
                                      style: const TextStyle(
                                        fontFamily: 'Poppins',
                                        fontSize: 14,
                                        fontWeight: FontWeight.w500,
                                        color: Colors.redAccent,
                                      ),
                                      textAlign: TextAlign.center,
                                    ),
                                  ),
                                ),
                              AnimatedScale(
                                scale: hasCheckedInToday ? 0.95 : 1.0,
                                duration: const Duration(milliseconds: 300),
                                child: Container(
                                  decoration: BoxDecoration(
                                    gradient: hasCheckedInToday
                                        ? null
                                        : const LinearGradient(
                                            colors: [gradientStart, gradientEnd],
                                            begin: Alignment.topLeft,
                                            end: Alignment.bottomRight,
                                          ),
                                    color: hasCheckedInToday ? Colors.grey.withOpacity(0.7) : null,
                                    borderRadius: BorderRadius.circular(12),
                                    boxShadow: hasCheckedInToday
                                        ? null
                                        : const [
                                            BoxShadow(
                                              color: Colors.black26,
                                              blurRadius: 6,
                                              offset: Offset(0, 3),
                                            ),
                                          ],
                                  ),
                                  child: Material(
                                    color: Colors.transparent,
                                    child: InkWell(
                                      borderRadius: BorderRadius.circular(12),
                                      onTap: customerID != null && !hasCheckedInToday
                                          ? _performCheckIn
                                          : null,
                                      child: Container(
                                        padding: const EdgeInsets.symmetric(
                                            horizontal: 40, vertical: 16),
                                        child: Center(
                                          child: Semantics(
                                            label: hasCheckedInToday
                                                ? 'Đã điểm danh hôm nay'
                                                : 'Điểm danh ngay',
                                            child: Text(
                                              hasCheckedInToday
                                                  ? 'Đã Điểm Danh Hôm Nay'
                                                  : 'Điểm Danh Ngay',
                                              style: const TextStyle(
                                                fontFamily: 'Poppins',
                                                fontSize: 18,
                                                fontWeight: FontWeight.w600,
                                                color: textColor,
                                              ),
                                            ),
                                          ),
                                        ),
                                      ),
                                    ),
                                  ),
                                ),
                              ),
                              const SizedBox(height: 16),
                              Container(
                                decoration: BoxDecoration(
                                  gradient: customerID == null
                                      ? null
                                      : const LinearGradient(
                                          colors: [gradientStart, gradientEnd],
                                          begin: Alignment.topLeft,
                                          end: Alignment.bottomRight,
                                        ),
                                  color: customerID == null ? Colors.grey.withOpacity(0.7) : null,
                                  borderRadius: BorderRadius.circular(12),
                                  boxShadow: customerID == null
                                      ? null
                                      : const [
                                          BoxShadow(
                                            color: Colors.black26,
                                            blurRadius: 6,
                                            offset: Offset(0, 3),
                                          ),
                                        ],
                                ),
                                child: Material(
                                  color: Colors.transparent,
                                  child: InkWell(
                                    borderRadius: BorderRadius.circular(12),
                                    onTap: customerID != null ? _navigateToPromotionScreen : null,
                                    child: Container(
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 40, vertical: 16),
                                      child: Center(
                                        child: Semantics(
                                          label: 'Xem khuyến mãi',
                                          child: Text(
                                            'Xem Khuyến Mãi',
                                            style: const TextStyle(
                                              fontFamily: 'Poppins',
                                              fontSize: 18,
                                              fontWeight: FontWeight.w600,
                                              color: textColor,
                                            ),
                                          ),
                                        ),
                                      ),
                                    ),
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 30),
                    FadeInUp(
                      duration: const Duration(milliseconds: 900),
                      child: const Text(
                        'Lịch Sử Điểm Danh',
                        style: TextStyle(
                          fontFamily: 'Poppins',
                          fontSize: 20,
                          fontWeight: FontWeight.w700,
                          color: textColor,
                          shadows: [
                            Shadow(color: Colors.black26, blurRadius: 4, offset: Offset(1, 1))
                          ],
                        ),
                      ),
                    ),
                    const SizedBox(height: 12),
                    FadeInUp(
                      duration: const Duration(milliseconds: 1000),
                      child: Card(
                        elevation: 8,
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                        color: primaryColor.withOpacity(0.3),
                        child: Container(
                          height: 400,
                          padding: const EdgeInsets.all(16),
                          child: TableCalendar(
                            firstDay: DateTime(2020),
                            lastDay: DateTime.now(),
                            focusedDay: DateTime.now(),
                            calendarFormat: CalendarFormat.month,
                            availableGestures: AvailableGestures.none,
                            headerStyle: const HeaderStyle(
                              formatButtonVisible: false,
                              titleTextStyle: TextStyle(
                                fontFamily: 'Poppins',
                                color: textColor,
                                fontSize: 18,
                                fontWeight: FontWeight.w700,
                              ),
                              leftChevronIcon: Icon(Icons.chevron_left, color: textColor, size: 28),
                              rightChevronIcon:
                                  Icon(Icons.chevron_right, color: textColor, size: 28),
                            ),
                            calendarStyle: CalendarStyle(
                              defaultTextStyle: const TextStyle(
                                fontFamily: 'Poppins',
                                color: textColor,
                                fontSize: 16,
                              ),
                              weekendTextStyle: const TextStyle(
                                fontFamily: 'Poppins',
                                color: textColor,
                                fontSize: 16,
                              ),
                              outsideTextStyle: TextStyle(
                                fontFamily: 'Poppins',
                                color: textColor.withOpacity(0.5),
                                fontSize: 16,
                              ),
                              todayDecoration: BoxDecoration(
                                color: hasCheckedInToday
                                    ? Colors.grey.withOpacity(0.7)
                                    : accentColor.withOpacity(0.4),
                                shape: BoxShape.circle,
                              ),
                              todayTextStyle: const TextStyle(
                                fontFamily: 'Poppins',
                                color: textColor,
                                fontWeight: FontWeight.w700,
                                fontSize: 16,
                              ),
                            ),
                            calendarBuilders: CalendarBuilders(
                              defaultBuilder: (context, date, _) {
                                final isCheckedIn = checkInDates.any((checkInDate) =>
                                    checkInDate.year == date.year &&
                                    checkInDate.month == date.month &&
                                    checkInDate.day == date.day);
                                if (isCheckedIn) {
                                  return FadeIn(
                                    duration: const Duration(milliseconds: 300),
                                    child: Container(
                                      margin: const EdgeInsets.all(4),
                                      decoration: BoxDecoration(
                                        gradient: LinearGradient(
                                          colors: [
                                            Colors.grey[600]!,
                                            Colors.grey[800]!,
                                          ],
                                          begin: Alignment.topLeft,
                                          end: Alignment.bottomRight,
                                        ),
                                        shape: BoxShape.circle,
                                        boxShadow: const [
                                          BoxShadow(
                                            color: Colors.black26,
                                            blurRadius: 4,
                                            offset: Offset(2, 2),
                                          ),
                                        ],
                                      ),
                                      child: Center(
                                        child: Text(
                                          '${date.day}',
                                          style: const TextStyle(
                                            color: textColor,
                                            fontFamily: 'Poppins',
                                            fontWeight: FontWeight.w600,
                                            fontSize: 16,
                                          ),
                                        ),
                                      ),
                                    ),
                                  );
                                }
                                return null;
                              },
                            ),
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
      ),
    );
  }

  Widget _buildShimmerContent() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Container(
          width: 200,
          height: 24,
          color: Colors.white,
        ),
        const SizedBox(height: 20),
        Container(
          width: 100,
          height: 100,
          decoration: const BoxDecoration(
            shape: BoxShape.circle,
            color: Colors.white,
          ),
        ),
        const SizedBox(height: 20),
        Container(
          width: double.infinity,
          height: 350, // Increased height for progress section, points display, and two buttons
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(16),
            color: Colors.white,
          ),
        ),
        const SizedBox(height: 20),
        Container(
          width: 200,
          height: 50,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(12),
            color: Colors.white,
          ),
        ),
        const SizedBox(height: 30),
        Container(
          width: 150,
          height: 24,
          color: Colors.white,
        ),
        const SizedBox(height: 12),
        Container(
          width: double.infinity,
          height: 400,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(16),
            color: Colors.white,
          ),
        ),
      ],
    );
  }
}