import 'package:flutter/material.dart';
import 'package:get_it/get_it.dart';
import 'package:barbermanagemobile/domain/usecases/get_branches_use_case.dart';
import 'package:barbermanagemobile/domain/entities/branch.dart';

class BranchScreen extends StatefulWidget {
  const BranchScreen({super.key});

  @override
  _BranchScreenState createState() => _BranchScreenState();
}

class _BranchScreenState extends State<BranchScreen> {
  final GetBranchesUseCase _getBranchesUseCase = GetIt.instance<GetBranchesUseCase>();

  static const primaryColor = Color(0xFF4E342E);
  static const backgroundColor = Color(0xFF212121);
  static const textColor = Color(0xFFEFEBE9);
  static const accentColor = Color(0xFF8D6E63);
  static const shadowColor = Color(0xFF3E2723);

  Future<void> _refreshBranches() async {
    await Future.delayed(Duration(seconds: 1));
    setState(() {});
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: backgroundColor,
      appBar: AppBar(
        title: Text(
          'Hệ thống chi nhánh',
          style: TextStyle(color: textColor, fontFamily: 'Poppins', fontWeight: FontWeight.bold),
        ),
        backgroundColor: primaryColor,
        elevation: 0,
        leading: IconButton(
          icon: Icon(Icons.arrow_back, color: textColor),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: RefreshIndicator(
        onRefresh: _refreshBranches,
        color: accentColor,
        child: FutureBuilder<List<Branch>>(
          future: _getBranchesUseCase.call(),
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)));
            } else if (snapshot.hasError) {
              return Center(child: Text('Lỗi: ${snapshot.error}', style: TextStyle(color: Colors.redAccent, fontSize: 14)));
            } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
              return Center(child: Text('Không có chi nhánh nào', style: TextStyle(color: textColor, fontSize: 14)));
            }

            final branches = snapshot.data!;
            return ListView.builder(
              physics: BouncingScrollPhysics(),
              padding: EdgeInsets.all(16),
              itemCount: branches.length,
              itemBuilder: (context, index) {
                final branch = branches[index];
                return Padding(
                  padding: const EdgeInsets.only(bottom: 12),
                  child: Card(
                    elevation: 5,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
                    child: Container(
                      decoration: BoxDecoration(
                        gradient: LinearGradient(
                          colors: [shadowColor, primaryColor.withOpacity(0.9)],
                          begin: Alignment.topLeft,
                          end: Alignment.bottomRight,
                        ),
                        borderRadius: BorderRadius.circular(15),
                        border: Border.all(color: accentColor.withOpacity(0.2), width: 0.5),
                      ),
                      child: Row(
                        children: [
                          ClipRRect(
                            borderRadius: BorderRadius.horizontal(left: Radius.circular(15)),
                            child: Image.network(
                              branch.branchImage,
                              width: 120,
                              height: 100,
                              fit: BoxFit.cover,
                              loadingBuilder: (context, child, loadingProgress) {
                                if (loadingProgress == null) return child;
                                return Center(child: CircularProgressIndicator(valueColor: AlwaysStoppedAnimation<Color>(accentColor)));
                              },
                              errorBuilder: (context, error, stackTrace) => Icon(Icons.broken_image, color: Colors.redAccent, size: 30),
                            ),
                          ),
                          Expanded(
                            child: Padding(
                              padding: const EdgeInsets.all(12.0),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    branch.branchName,
                                    style: TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.bold,
                                      color: textColor,
                                      fontFamily: 'Poppins',
                                    ),
                                    maxLines: 1,
                                    overflow: TextOverflow.ellipsis,
                                  ),
                                  SizedBox(height: 4),
                                  Text(
                                    branch.location,
                                    style: TextStyle(
                                      fontSize: 12,
                                      color: textColor.withOpacity(0.8),
                                      fontFamily: 'Poppins',
                                    ),
                                    maxLines: 2,
                                    overflow: TextOverflow.ellipsis,
                                  ),
                                  SizedBox(height: 4),
                                  Text(
                                    'Hotline: ${branch.branchHotline}',
                                    style: TextStyle(
                                      fontSize: 12,
                                      color: textColor.withOpacity(0.8),
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                  SizedBox(height: 4),
                                  Text(
                                    'Giờ làm việc: ${branch.startWork} - ${branch.endWork}',
                                    style: TextStyle(
                                      fontSize: 12,
                                      color: accentColor,
                                      fontFamily: 'Poppins',
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                );
              },
            );
          },
        ),
      ),
    );
  }
}