import 'package:barbermanagemobile/domain/entities/vip.dart';

abstract class VipRepository {
  Future<List<Vip>> getVips();
}